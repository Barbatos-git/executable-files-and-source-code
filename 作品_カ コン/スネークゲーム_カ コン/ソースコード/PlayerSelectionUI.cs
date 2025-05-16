using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerSelectionUI : MonoBehaviour
{
    public Image maleBGImage;
    public Button maleButton;
    public Image femaleBGImage;
    public Button femaleButton;
    public Image maleImage;
    public Image femaleImage;

    private Color defaultColor;
    private Color fadeColor = Color.gray;

    //private bool isMaleClicked = false;
    //private bool isFemaleClicked = false;

    private string selectedGender = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // デフォルトの色を取得
        defaultColor = maleButton.GetComponent<Image>().color;

        // 初期状態をグレーに設定
        maleButton.GetComponent<Image>().color = fadeColor;
        maleBGImage.color = fadeColor;
        maleImage.color = fadeColor;
        femaleButton.GetComponent<Image>().color = fadeColor;
        femaleBGImage.color = fadeColor;
        femaleImage.color = fadeColor;

        // 全ての PlayerPrefs データを削除
        PlayerPrefs.DeleteAll();

        // 初回起動時（保存がない場合）、デフォルトの "Male" を保存
        selectedGender = PlayerPrefs.GetString("SelectedGender", "Male");

        // 初回起動時（保存がない場合）、デフォルトの "Male" を保存
        if (!PlayerPrefs.HasKey("SelectedGender"))
        {
            PlayerPrefs.SetString("SelectedGender", "Male");
            PlayerPrefs.Save();
        }

        // ボタンの状態を初期化
        UpdateButtonStates();

        // ボタンのイベントリスナーを追加
        AddButtonEvents();

        // デフォルトで男性を選択
        //SelectMale();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボタンのイベントリスナーを追加
    private void AddButtonEvents()
    {
        //// Male Button Events
        //maleButton.onClick.AddListener(() => SelectMale());

        //// Female Button Events
        //femaleButton.onClick.AddListener(() => SelectFemale());

        maleButton.onClick.AddListener(() => OnGenderSelected("Male"));
        femaleButton.onClick.AddListener(() => OnGenderSelected("Female"));
    }

    //// 男性キャラを選択する処理
    //private void SelectMale()
    //{
    //    if (!isMaleClicked)
    //    {
    //        isMaleClicked = true;
    //        maleButton.GetComponent<Image>().color = defaultColor;
    //        maleImage.color = defaultColor;
    //        maleBGImage.color = defaultColor;

    //        // 女性ボタンをグレーに戻す
    //        isFemaleClicked = false;
    //        femaleButton.GetComponent<Image>().color = fadeColor;
    //        femaleImage.color = fadeColor;
    //        femaleBGImage.color = fadeColor;

    //        // 性別選択を保存
    //        PlayerPrefs.SetString("SelectedGender", "Male");
    //    }
    //}

    //// 女性キャラを選択する処理
    //private void SelectFemale()
    //{
    //    if (!isFemaleClicked)
    //    {
    //        isFemaleClicked = true;
    //        femaleButton.GetComponent<Image>().color = defaultColor;
    //        femaleImage.color = defaultColor;
    //        femaleBGImage.color = defaultColor;

    //        // 男性ボタンをグレーに戻す
    //        isMaleClicked = false;
    //        maleButton.GetComponent<Image>().color = fadeColor;
    //        maleImage.color = fadeColor;
    //        maleBGImage.color = fadeColor;

    //        // 性別選択を保存
    //        PlayerPrefs.SetString("SelectedGender", "Female");
    //    }
    //}

    void OnGenderSelected(string gender)
    {
        selectedGender = gender;
        PlayerPrefs.SetString("SelectedGender", selectedGender);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        if (selectedGender == "Male")
        {
            SetButtonState(maleButton, maleImage, maleBGImage, true);
            SetButtonState(femaleButton, femaleImage, femaleBGImage, false);
        }
        else if (selectedGender == "Female")
        {
            SetButtonState(maleButton, maleImage, maleBGImage, false);
            SetButtonState(femaleButton, femaleImage, femaleBGImage, true);
        }
        else
        {
            // まだ選択されていない場合
            SetButtonState(maleButton, maleImage, maleBGImage, false);
            SetButtonState(femaleButton, femaleImage, femaleBGImage, false);
        }
    }

    void SetButtonState(Button button, Image image, Image bgImage, bool isSelected)
    {
        if (isSelected)
        {
            button.GetComponent<Image>().color = defaultColor;
            image.color = defaultColor;
            bgImage.color = defaultColor;
        }
        else
        {
            button.GetComponent<Image>().color = fadeColor;
            image.color = fadeColor;
            bgImage.color = fadeColor;
        }
    }
}
