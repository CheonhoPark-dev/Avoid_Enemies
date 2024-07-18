using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image keyImage;
    public static UIManager instance;
    public bool pickKey;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pickKey = false;
        keyImage.enabled = pickKey; // 시작할 때 열쇠 아이콘 비활성화
    }

    public void ShowKey()
    {
        pickKey = true;
        keyImage.enabled = true; // 열쇠 아이콘 활성화
    }

    public bool isKey()
    {
        return pickKey;
    }
}
