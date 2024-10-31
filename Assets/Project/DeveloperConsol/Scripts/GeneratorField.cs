using UnityEngine;
using TMPro;

public class GeneratorField : MonoBehaviour
{
    [SerializeField] private string _generatorName;

    private GeneratorManager _generatorManager;
    private TMP_InputField _field;

    public void Init(GeneratorManager generatorManager)
    {
        _field = GetComponent<TMP_InputField>();
        _generatorManager = generatorManager;
        _field.onEndEdit.AddListener(ChangeGeneratorLvl);
    }

    public void ChangeGeneratorLvl(string lvl)
    {
        if (int.TryParse(lvl, out int result))
        {
            _generatorManager.GeneratorSetLvl(_generatorName, result);
        }
    }
}
