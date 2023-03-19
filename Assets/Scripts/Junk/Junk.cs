using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class Junk : MonoBehaviour
{
    [SerializeField] private Color _lockedColor;

    public int Level { get; private set; }
    public int Amount { get; private set; }
    public bool IsLocked { get; private set; }
    public BoxCollider AttachedCollider => _attachedBoxCollider;

    public event UnityAction<Junk> JunkCollected;

    private MeshRenderer _meshRenderer;
    private Color[] _materialsColors;
    private PlayerLevel _playerLevel;
    private BoxCollider _attachedBoxCollider;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = _meshRenderer.materials;
        _materialsColors = new Color[materials.Length];
        _attachedBoxCollider = GetComponent<BoxCollider>();

        for (int i = 0; i < materials.Length; i++)
            _materialsColors[i] = materials[i].color;
    }

    public void Init(int level, int amount, PlayerLevel playerLevel)
    {
        if (level < 1)
        {
            Debug.LogError("Junk's level can't be less then one!");
            return;
        }

        if (amount < 1)
        {
            Debug.LogError("Junk's amount can't be less then one!");
            return;
        }

        Level = level;
        Amount = amount;
        _playerLevel = playerLevel;
        _playerLevel.LevelChanged += UpdateIsLocked;
        UpdateIsLocked(playerLevel.Level);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Unlock()
    {
        IsLocked = false;
        UpdateColor();
    }

    public void Collected()
    {
        if (IsLocked == true)
            return;

        Destroy(gameObject);
        JunkCollected?.Invoke(this);
    }

    public void NotCollected()
    {
        
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void UpdateColor()
    {
        if (_meshRenderer == null)
        {
            return;
        }

        if (IsLocked)
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
                _meshRenderer.materials[i].color = _lockedColor;
        else
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
                _meshRenderer.materials[i].color = _materialsColors[i];
    }

    private void UpdateIsLocked(int currentPlayerLevel)
    {
        if (currentPlayerLevel < 1)
        {
            Debug.LogError("Player's level can't be less then one!");
            return;
        }

        if (currentPlayerLevel < Level)
        {
            IsLocked = true;
            UpdateColor();
            return;
        }

        Unlock();
        _playerLevel.LevelChanged -= UpdateIsLocked;
    }
}
