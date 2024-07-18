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
        keyImage.enabled = pickKey; // ������ �� ���� ������ ��Ȱ��ȭ
    }

    public void ShowKey()
    {
        pickKey = true;
        keyImage.enabled = true; // ���� ������ Ȱ��ȭ
    }

    public bool isKey()
    {
        return pickKey;
    }
}
