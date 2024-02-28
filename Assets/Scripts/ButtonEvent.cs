using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ivy.MiniGame;
using Ivy.Cloud;
using UnityEngine.UI;
using UnityEngine.Diagnostics;
using com.Ivy;

public class ButtonEvent : MonoBehaviour
{


    void Awake()
    {
        // IvySdk.Instance.Init();


        IvySdkListener.OnTencentCloudFunctionSuccess += (string api, string data) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} data:{data}");
        };

        IvySdkListener.OnTencentCloudFunctionFail += (string api) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} failed");
        };

        // RiseSdkListener.OnAdEvent += 

        IvySdkListener.OnPaymentEvent += (IvySdk.PaymentResult result, int i, string str) =>
        {
            Ivy.Utils.Log.Print($"pay result::{result} {i} {str}");
        };

        IvySdkListener.OnPaymentWithPayloadEvent += (IvySdk.PaymentResult result, int i, string str, string str1) =>
        {
            Ivy.Utils.Log.Print($"pay result::{result} {i} {str} {str1}");
        };

    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.Find("IsLogin").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            IvySdk.Instance.IsLogin((state) =>
            {

            });
            // Debug.Log($"has login wechat? {MiniSdk.IsPreLogin}");
        });

        this.transform.Find("login").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            IvySdk.Instance.Login((data) => { }, (err) => { });
        });

        this.transform.Find("Me").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            string str = IvySdk.Instance.LoggedUser();
            Debug.Log($"me::{str}");
        });

        // this.transform.Find("logout").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        // {

        // });

        this.transform.Find("Pay").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            IvySdk.Instance.Pay(1);
        });

        this.transform.Find("GetPay").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            string str = IvySdk.Instance.GetPaymentData(1);
            Ivy.Utils.Log.Print($"get pay data success: {str}");
        });

        this.transform.Find("service").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            IvySdk.Instance.ShowHelp();
        });

        this.transform.Find("system").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            bool state = IvySdk.Instance.IsIOSSystem();
            Ivy.Utils.Log.Print($"is ios system? {state}");
        });

        this.transform.Find("CloudFunc").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            Dictionary<string, object> _dict = new Dictionary<string, object>()
            {
                {"appid", Ivy.MiniGame.MiniGameConfig.appid},
                {"uuid", Ivy.MiniGame.MiniGameConfig.openId},
                {"login_code", Ivy.MiniGame.MiniGameConfig.LoginCode},
            };
            IvySdk.Instance.CallTencentCloudFunction("api_timestamp", _dict.ToString(), (data) =>
            {

            }, (api_key) =>
            {

            });
        });

    }

    // Update is called once per frame
    void Update()
    {

    }



}
