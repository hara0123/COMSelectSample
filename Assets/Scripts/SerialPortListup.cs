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
    public List<string> COMPortName_;
    public List<string> COMPortDetail_;

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
            portNum = ExistingCOMPort_;
        }
        else
        {
            if (e.Data[0] == 'N')
            {
                COMPortName_.Add(e.Data.Substring(1)); // 先頭1文字（'N'）を削除
            }
            else if (e.Data[0] == 'D')
            {
                COMPortDetail_.Add(e.Data.Substring(1)); // 先頭1文字（'D'）を削除
            }
        }
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
