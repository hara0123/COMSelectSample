using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.LightingExplorerTableColumn;

public class SerialPortListup : MonoBehaviour
{
    public int portNum { get; private set; }
    public string[] portName { get; private set; }

    Process process_;
    static readonly string FolderPath = Application.streamingAssetsPath + "/Apps";
    static readonly string FilePath = FolderPath + "/SerialPortName.exe";

    int ExistingCOMPort_;
    List<string> COMPortName_;
    List<string> COMPortDetail_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        portName = SerialPort.GetPortNames();
        portNum = portName.Length;

        ExistingCOMPort_ = -1;
        COMPortName_ = new List<string>();
        COMPortDetail_ = new List<string>();

        process_ = new Process();

        // プロセスを起動するときに使用する値のセットを指定
        process_.StartInfo = new ProcessStartInfo
        {
            FileName = FilePath,                        // 起動するファイルのパスを指定する
            UseShellExecute = false,                    // プロセスの起動にオペレーティング システムのシェルを使用するかどうか(既定値:true)
            WorkingDirectory = FolderPath,              // 開始するプロセスの作業ディレクトリを取得または設定する(既定値:"")
            RedirectStandardInput = true,               // StandardInput から入力を読み取る(既定値：false)
            RedirectStandardOutput = true,              // 出力を StandardOutput に書き込むかどうか(既定値：false)
            CreateNoWindow = true,                      // プロセス用の新しいウィンドウを作成せずにプロセスを起動するかどうか(既定値：false)
            //StandardOutputEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.GetEncoding("shift-jis"),
        };

        // 外部プロセスのStandardOutput ストリームに行を書き込む度に発火されるイベント
        process_.OutputDataReceived += OnStandardOut;

        //外部プロセスの終了を検知する
        process_.EnableRaisingEvents = true;
        process_.Exited += DisposeProcess;

        // プロセスを起動する
        process_.Start();
        process_.BeginOutputReadLine();
    }

    void OnStandardOut(object sender, DataReceivedEventArgs e)
    {
        if (ExistingCOMPort_ == -1)
        {
            int.TryParse(e.Data, out ExistingCOMPort_);
        }
        else
        {
            //if
        }

            COMPortName_.Add(e.Data);



        //DataType type = CheckDataType(e.Data);

        //switch (type)
        //{
        //    case DataType.Number:
        //        UnityEngine.Debug.Log("aaaa");
        //        break;
        //    case DataType.Name:
        //        string temp2 = "あいう";
        //        //portList.Add(e.Data);
        //        portList.Add(temp2);
        //        UnityEngine.Debug.Log("bbbb");
        //        break;
        //    case DataType.Detail:
        //        //detailList.Add(e.Data);
        //        string temp = "えおか";

        //        detailList.Add(temp);
        //        UnityEngine.Debug.Log("cccc");
        //        break;
        //    default:
        //        UnityEngine.Debug.Log("oooo");
        //        break;
        //}
        UnityEngine.Debug.Log(e.Data);


    }


    void DisposeProcess(object sender, EventArgs e)
    => DisposeProcess();

    void DisposeProcess()
    {
        if (process_ == null || process_.HasExited) return;

        process_.StandardInput.Close();
        process_.CloseMainWindow();
        process_.Dispose();
        process_ = null;
    }

}
