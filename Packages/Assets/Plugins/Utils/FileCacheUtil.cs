using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if USE_IVY_LOCAL_PROTO
using Google.Protobuf;
#endif

namespace Ivy.Utils
{
    [Serializable]
    internal class FileLister : ISerializationCallbackReceiver
    {
        [NonSerialized] internal HashSet<string> FileHash = new HashSet<string>();
        [SerializeField] private List<string> fileNameArray = new List<string>();

        [NonSerialized] internal Dictionary<string,string> LocalDatabase = new Dictionary<string,string>();
        [SerializeField] private List<string> keyArray = new List<string>();
        [SerializeField] private List<string> contentArray = new List<string>();
        
        public void OnBeforeSerialize()
        {
            fileNameArray.Clear();
            foreach (var fileName in FileHash)
            {
                fileNameArray.Add(fileName);
            }
            keyArray.Clear();
            contentArray.Clear();
            foreach (var kp in LocalDatabase)
            {
                keyArray.Add(kp.Key);
                contentArray.Add(kp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            FileHash.Clear();
            foreach (var fileName in fileNameArray)
            {
                FileHash.Add(fileName);
            }
            LocalDatabase.Clear();
            for (var i = 0; (i < keyArray.Count) && (i < contentArray.Count) ;++i)
            {
                LocalDatabase[keyArray[i]] = contentArray[i];
            }
        }
    }
    
    public static class FileCacheUtil 
    {
        #region 字段定义
        
        private static bool _isInit = false;
        //
        private static readonly ConcurrentDictionary<string, byte[]> cacheDict = new ConcurrentDictionary<string, byte[]>();
        //
        private static FileLister _mFileList = new FileLister();
        
        private const string FileSuffix = ".fbs";
        private const string KeyCode = "4$byRkKy5U@efn3y";

        #endregion
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadCacheFileName()
        {
            //程序初始化运行
            if(_isInit) return;
            _isInit = true;
            
            var listName = GETConfigPath();
            if(!File.Exists(listName)) return;
            
            var content = ReadFileToString(listName);
            if (content.Length > 0)
            {
                _mFileList = JsonUtility.FromJson<FileLister>(content);
            }
            
            foreach (var fileName in _mFileList.FileHash)
            {
                var cacheFilePath = GETFilePath(fileName);
                if(!File.Exists(cacheFilePath)) continue;
                cacheDict[fileName] = ReadFile(cacheFilePath);
            }
        }

        public static bool IsAllSaved()
        {
            return OperatorDispatcher.I.IsAllSaved();
        }
        
#if USE_IVY_LOCAL_PROTO
        //保存protoBuf
        public static void SaveProto<T>(string fileName, T message) where T : IMessage
        {
            if (message == null) return;

            var byteArray = message.ToByteArray();
            cacheDict[fileName] = byteArray;
            
            //异步检查文件索引和写入文件
            OperatorDispatcher.I.AddActionToQueue(() =>
            {
                WriteLocalCache(fileName, byteArray);
            });
        }

        //读取protoBuf
        public static void ReadProto<T>(string fileName, out T message, out bool isExisted) where T : IMessage
        {
            //根据类型创建消息
            message = Activator.CreateInstance<T>();
            isExisted = false;
            //如果缓存中没有返回空消息
            if (!cacheDict.TryGetValue(fileName, out var byteArray)) return;
            try
            {
                message.MergeFrom(byteArray);
                isExisted = true;
            }
            catch (Exception )
            {
                isExisted = false;
            }

        }
#endif
        
        //保存内容
        public static void WriteContent(string fileName, string content)
        {
            if (content == null) return;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            cacheDict[fileName] = byteArray;
            
            //异步检查文件索引和写入文件
            OperatorDispatcher.I.AddActionToQueue(() =>
            {
                WriteLocalCache(fileName, byteArray);
            });
        }
        
        //读取内容
        public static void ReadContent(string fileName, out string message, out bool isExisted)
        {
            message = null;
            isExisted = false;
            //如果缓存中没有返回空消息
            if (!cacheDict.TryGetValue(fileName, out var byteArray)) return;
            isExisted = true;
            message =  System.Text.Encoding.UTF8.GetString(byteArray);
        }
        
        //本地保存key-value pair
        public static void WriteLocal(string key, string value)
        {
            OperatorDispatcher.I.AddActionToQueue(() =>
            {
                _mFileList.LocalDatabase[key] = value;
                var listPath = GETConfigPath();
                WriteFile(listPath, JsonUtility.ToJson(_mFileList));
            });
        }

        //本地读取key-value pair
        public static bool ReadLocal(string key, out string msg)
        {
            msg = "";
            if (!_mFileList.LocalDatabase.TryGetValue(key, out var ret)) return false;
            msg = ret;
            return true;
        }
        
         #region 私有工具函数
        
        private static string GETConfigPath()
        {
            var cacheFolder = GetCacheFolder();
            return $"{cacheFolder}/FileCache.ini";
        }

        private static string GETFilePath(string fileName)
        {
            var cacheFolder = GetCacheFolder();
            return $"{cacheFolder}/{fileName}{FileSuffix}";
        }
        
        //获取缓存路径
        private static string GetCacheFolder()
        {
            var cacheFolder = Path.Combine(Application.persistentDataPath, "IvyCacheData");
            if (!Directory.Exists(cacheFolder)) Directory.CreateDirectory(cacheFolder);
            return cacheFolder;
        }
        
        private static void WriteFile(string filePath, string content)
        {
            WriteFile(filePath, System.Text.Encoding.UTF8.GetBytes(content));
        }

        private static void WriteFile(string filePath, byte[] byteArray)
        {
            try
            {
                byteArray = Xxtea.XXTEA.Encrypt(byteArray, KeyCode);
                File.WriteAllBytes(filePath, byteArray);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static byte[] ReadFile(string filePath)
        {
            var ret = new byte[] { };
            try
            {
                var cacheByteArray = File.ReadAllBytes(filePath);
                ret = Xxtea.XXTEA.Decrypt(cacheByteArray, KeyCode);
            }
            catch (Exception)
            {
                // ignored
            }

            return ret;
        }

        private static string ReadFileToString(string filePath)
        {
            return System.Text.Encoding.UTF8.GetString(ReadFile(filePath));
        }
        
        private static void WriteLocalCache(string fileName, byte[] byteArray)
        {
            var cacheFilePath = GETFilePath(fileName);
            WriteFile(cacheFilePath, byteArray);
                
            if (_mFileList.FileHash.Contains(fileName)) return;
            _mFileList.FileHash.Add(fileName);
                
            var listPath = GETConfigPath();
            var content = JsonUtility.ToJson(_mFileList);
            WriteFile(listPath, content);
        }
        #endregion

    }

}
