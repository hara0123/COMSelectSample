using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    SerialPortListup serialPortListup_;
    public GameObject buttonPrefab_;
    public Transform canvasTransform_;
    public TMP_FontAsset japaneseFont_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPortListup_ = GetComponent<SerialPortListup>();
        for (int i = 0; i < serialPortListup_.portNum; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab_, canvasTransform_);

            // 位置をずらして配置
            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 * i);

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

        int exitIndex = 3;
        exitButtonComp.onClick.AddListener(() => OnButtonClicked(exitIndex)); // iを直接渡すと全てのボタンに最後の値が渡る。クロージャ問題。
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Debug.Log("index 2");
                break;
            case 3:
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
