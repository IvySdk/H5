### 结构说明:
    1. 小游戏sdk: Assets/Plugins/IvyMiniGame/  
    2. IvySDK: Assets/Plugins/IvySdk/
    3. 工具: Assets/Plugins/Utils/
    4. 抖音sdk: Assets/Plugins/ByteGame/
    
## 1. 导入微信小游戏sdk
    1. [导入微信小游戏sdk](https://gitee.com/wechat-minigame/minigame-unity-webgl-transform#https://gitee.com/link?target=https%3A%2F%2Fgame.weixin.qq.com%2Fcgi-bin%2Fgamewxagwasmsplitwap%2Fgetunityplugininfo%3Fdownload%3D1)
	2. 导入 sdk
	3. 添加宏CN_WX
	
## 2. 接口说明

### 1. 游戏配置
请参考 Assets/Plugins/IvyMiniGameAssets/config/MiniGameConfig.cs


### 2. 微信相关
#### 微信登陆
```javascript
RiseSdkListener.Event_LoginResult += (int state, string str) =>
{
	if (state == 1)
	{
		Ivy.Utils.Log.Print("login success");
	}
	else
	{
		Ivy.Utils.Log.Print("login fail");
	}
};

 RiseSdk.Instance.Login();
```
#### 微信登陆状态判断
```javascript
 RiseSdkListener.Event_CheckPreLoginState += (bool state) =>
 {
	Ivy.Utils.Log.Print($"log state::{state}");
};
		
RiseSdk.Instance.IsLogin();
```
#### 登陆用户信息
```javascript
string str = RiseSdk.Instance.Me();
```

### 3. 计费相关
#### 获取计费点信息
```javascript
string str = RiseSdk.Instance.GetPaymentData(billId);
```
#### 支付
```javascript
string str = RiseSdk.Instance.GetPaymentData(billId);
```
#### 计费点配置
请参考示例中的 payConfig.json
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
RiseSdk.Instance.ShowAIHelp("", "");
```

### 5. 系统环境
#### 是否为IOS
```javascript
bool state = RiseSdk.Instance.IsIOSSystem();
```

### 6. 云函数
#### 云函数api说明文档
参照 [腾讯云函数接口文档(1).docx](file://腾讯云函数接口文档(1).docx)

#### 使用示例
```javascript
        RiseSdkListener.OnCloudFunctionResult += (string api, string data) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} data:{data}");
        };

        RiseSdkListener.OnCloudFunctionFailed += (string api) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} failed");
        };

            Dictionary<string, object> _dict = new Dictionary<string, object>()
            {
                {"appid", Ivy.MiniGame.MiniGameConfig.appid},
                {"uuid", Ivy.MiniGame.MiniGameConfig.openId},
                {"login_code", Ivy.MiniGame.MiniGameConfig.LoginCode},
            };
            RiseSdk.Instance.CallTxCloudFunc("api_timestamp", _dict.ToString());
```
