### 结构说明:
    1. 小游戏sdk: Assets/Plugins/IvyMiniGame/  
    2. IvySDK: Assets/Plugins/IvySdk/
    3. 工具: Assets/Plugins/Utils/
    4. 抖音sdk: Assets/Plugins/ByteGame/
    
## 1. 使用方式
##### I. 安装微信小游戏sdk
1. Package方式安装
打开游戏工程 -> Unity Editor 菜单栏 -> Package Manager -> 右上方“+” -> Add Package from git URL
URL 地址为：
```javascript
https://github.com/wechat-miniprogram/minigame-tuanjie-transform-sdk.git
```
详细文档请参考
```javascript
https://gitee.com/wechat-minigame/minigame-unity-webgl-transform#/wechat-minigame/minigame-unity-webgl-transform/blob/main/Design/SDKInstaller.md
```
##### II. 导入 sdk
##### III.  添加宏 IVYSDK_WX
	
## 2. 接口说明

### 1. 游戏配置
请参考 [MiniGameConfig.cs](Docs/Assets/Plugins/IvyMiniGameAssets/config/MiniGameConfig.cs)


### 2. 微信相关
#### 微信登陆
```javascript
IvySdk.Instance.Login((data) => { 
//登陆成功
}, (err) => { 
//登陆失败
});
```
#### 微信登陆状态判断
```javascript
IvySdk.Instance.IsLogin((state) =>
{

});
```
##### 登陆示例
```javascript
IvySdk.Instance.IsLogin((state) =>
{
	if(!state){
			IvySdk.Instance.Login((data) => { 
			//登陆成功
			}, (err) => { 
			//登陆失败
			});
	}
});
```

#### 登陆用户信息
```javascript
string str = IvySdk.Instance.LoggedUser();
```
##### 微信用户信息示例
```javascript
{
	"logInfo":{
		"openId":"",
		"unionId":""
	}
}
```


### 3. 计费相关
计费点信息需要提前录入管理后台，否则拉起支付时会出现异常，请联系运营人员录入

#### 获取计费点信息
```javascript
string str = IvySdk.Instance.GetPaymentData(billId);
```
#### 支付
```javascript
IvySdk.Instance.Pay(billId);
string str = IvySdk.Instance.Pay(billId, payload);
```

#### 计费回调
```javascript
IvySdkListener.OnPaymentEvent += (IvySdk.PaymentResult result, int i, string str) =>
{
	Ivy.Utils.Log.Print($"pay result::{result} {i} {str}");
};

IvySdkListener.OnPaymentWithPayloadEvent += (IvySdk.PaymentResult result, int i, string str, string str1) =>
{
	Ivy.Utils.Log.Print($"pay result::{result} {i} {str} {str1}");
};
```


#### 计费点配置
请参考示例中的 [payConfig.json](Doc/Assets/Resources/payConfig.json)
```javascript
{
    "1": {
        "title": "20钻石",
        "productId": "1",
        "currency": "CNY",
        "price": "6元(测试)",
        "feeValue": 100.0,
        "rmb": 1,
        "amount": 100,
        "desc": "20钻石"
    },
    "2": {
        "title": "100钻石",
        "productId": "2",
        "currency": "CNY",
        "price": "30元(测试)",
        "feeValue": 100.0,
        "rmb": 1,
        "amount": 100,
        "desc": "100钻石"
    }
}
```

### 4. 客服
#### 打开微信客服
```javascript
IvySDK.Instance.ShowHelp();
```

### 5. 系统环境
#### 是否为IOS
```javascript
bool state = IvySDK.Instance.IsIOSSystem();
```

### 6. 事件
```javascript
IvySDK.Instance.LogEvent(string event, string keyValueData);
```


### 7. 云函数
#### 云函数api说明文档
参照 [腾讯云函数接口文档(1).docx](docs/腾讯云函数接口文档(1).docx)

#### 使用示例
```javascript
        IvySdkListener.OnTencentCloudFunctionSuccess += (string api, string data) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} data:{data}");
        };

        IvySdkListener.OnTencentCloudFunctionFail += (string api) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} failed");
        };

 		IvySdk.Instance.CallTencentCloudFunction(api_key,  param);
```
