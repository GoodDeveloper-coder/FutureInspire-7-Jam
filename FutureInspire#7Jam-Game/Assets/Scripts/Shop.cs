using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [SerializeField] private InputActionReference _openShopKey;
    [SerializeField] private float _minRadiusToOpenShop = 10f;
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private GameObject _shop;
    [SerializeField] private Transform _player;
    [SerializeField] private List<GameObject> _buyButtons;
    [SerializeField] private List<GameObject> _equipButtons;
    [SerializeField] private List<GameObject> _equipedButtons;
    [SerializeField] private List<int> _swordsPrice = new List<int>();
    [SerializeField] private List<int> _swordsDamage = new List<int>();
    [SerializeField] private Sword _sword;
    [SerializeField] private SkinnedMeshRenderer _swordSkinnedMeshRenderer;
    [SerializeField] private List<Material> _swordsMaterials;

    void Start()
    {
        _openShopKey.action.started += (InputAction.CallbackContext context)=> OpenShop();
        LoadShop();
    }

    void Update()
    {
        if (_shopUI.activeSelf)
        {
            if (Vector3.Distance(_shop.transform.position, _player.position) >= _minRadiusToOpenShop)
            {
                _shopUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void OpenShop()
    {
        if (!_shopUI.activeSelf)
        {
            if (Vector3.Distance(_shop.transform.position, _player.position) < _minRadiusToOpenShop)
            {
                _shopUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                UpdateShop();
            }
        }
        else
        {
            _shopUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void BuySword(int swordIndex)
    {
        if (GameManager._instance._coins >= _swordsPrice[swordIndex - 1])
        {
            PlayerPrefs.SetInt("Sword" + swordIndex, 1);
            GameManager._instance.RemoveCoins(_swordsPrice[swordIndex - 1]);
            UpdateShop();
        }
    }

    public void EquipSword(int swordIndex)
    {
        PlayerPrefs.SetInt("EquipedSword", swordIndex);
        UpdateShop();
        UpdateEquipedSword();
    }

    void UpdateShop()
    {
        for (int i = 0; i < _buyButtons.Count; i++)
        {
            if (PlayerPrefs.HasKey("Sword" + (i + 1)))
            {
                _equipButtons[i].SetActive(true);
                _buyButtons[i].SetActive(false);
                _equipedButtons[i].SetActive(false);
            }
            else
            {
                _buyButtons[i].SetActive(true);
                _equipButtons[i].SetActive(false);
                _equipedButtons[i].SetActive(false);
            }

            if (PlayerPrefs.HasKey("EquipedSword"))
            {
                if (i == PlayerPrefs.GetInt("EquipedSword") - 1)
                {
                    _equipedButtons[i].SetActive(true);
                    _equipButtons[i].SetActive(false);
                    _buyButtons[i].SetActive(false);
                }
            }
        }
    }

    void LoadShop()
    {
        UpdateShop();
        UpdateEquipedSword();
    }

    void UpdateEquipedSword()
    {
        if (PlayerPrefs.HasKey("EquipedSword"))
        {
            int swordIndex = PlayerPrefs.GetInt("EquipedSword") - 1;
            _sword.SetDamage(_swordsDamage[swordIndex]);
            Material[] materials = _swordSkinnedMeshRenderer.materials;
            materials[0] = _swordsMaterials[swordIndex];
            _swordSkinnedMeshRenderer.materials = materials;
        }
    }

    void OnDestroy()
    {
        _openShopKey.action.started -= (InputAction.CallbackContext context)=> OpenShop();
    }
}
