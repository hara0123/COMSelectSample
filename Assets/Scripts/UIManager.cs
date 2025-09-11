using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    SerialPortListup serialPortListup_;
    int selectedIndex_;

    public GameObject buttonPrefab_;
    public Transform canvasTransform_;
    public TMP_FontAsset japaneseFont_;
    public GameObject USBImagePrefab_;

    SignalChangeDetector signalChangeDetector_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPortListup_ = GetComponent<SerialPortListup>();
        for (int i = 0; i < serialPortListup_.portNum; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab_, canvasTransform_);
            // 位置をずらして配置
            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 * i);

            if (i == 0)
            {
                USBImagePrefab_ = Instantiate(USBImagePrefab_, canvasTransform_);
                USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = buttonObj.GetComponent<RectTransform>().anchoredPosition + new Vector2(-150, 0);
            }

            // ボタンのテキストを変更
            Button buttonComp = buttonObj.GetComponent<Button>();
            TextMeshProUGUI label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            label.text = serialPortListup_.portName[i]; // ボタンの文字
            label.color = Color.white; // ボタンの文字の色
            label.font = japaneseFont_; // ボタンの文字のフォント

            int index = i;
            buttonComp.onClick.AddListener(() => OnButtonClicked(index)); // iを直接渡すと全てのボタンに最後の値が渡る。クロージャ問題。
        }

        GameObject exitButtonObj = Instantiate(buttonPrefab_, canvasTransform_);

        exitButtonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -500);

        // ボタンのテキストを変更
        Button exitButtonComp = exitButtonObj.GetComponent<Button>();
        TextMeshProUGUI exitLabel = exitButtonObj.GetComponentInChildren<TextMeshProUGUI>();
        exitLabel.text = "終了"; // ボタンの文字
        exitLabel.color = Color.white; // ボタンの文字の色
        exitLabel.font = japaneseFont_; // ボタンの文字のフォント

        exitButtonComp.onClick.AddListener(() => OnButtonClicked(serialPortListup_.portNum));

        selectedIndex_ = 0;
        signalChangeDetector_ = new SignalChangeDetector(selectedIndex_);
    }

    // Update is called once per frame
    void Update()
    {
        var current = Keyboard.current;

        if (current.upArrowKey.wasPressedThisFrame)
            selectedIndex_--;
        if (current.downArrowKey.wasPressedThisFrame)
            selectedIndex_++;
        if (current.enterKey.wasPressedThisFrame)
        {
            OnButtonClicked(selectedIndex_);
        }

        if (selectedIndex_ < 0)
        {
            selectedIndex_ = 0;
        }
        else if (selectedIndex_ > serialPortListup_.portNum) // exitボタンがあるので条件の書き方はOK
        {
            selectedIndex_ = serialPortListup_.portNum;
        }

        signalChangeDetector_.Input(selectedIndex_);

        if (signalChangeDetector_.IsChanged())
        {
            USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -100 * selectedIndex_);
        }

        signalChangeDetector_.Update();
    }

    void OnButtonClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                Debug.Log("index 0");
                break;
            case 1:
                Debug.Log("index 1");
                break;
            case 2:
                // exit
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }
}
