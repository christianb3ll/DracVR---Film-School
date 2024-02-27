using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;

public class EmotionManager : MonoBehaviour
{
    [Range(-100, 100)]
    public int smile;

    [Range(-100, 100)]
    public int sad;

    [Range(-100, 100)]
    public int surprise;

    [Range(-100, 100)]
    public int thoughtful;

    [Range(-100, 100)]
    public int anger;

    private CharacterCustomization character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterCustomization>();
    }

    // Update is called once per frame
    void Update()
    {
        character.SetBlendshapeValue(CharacterBlendShapeType.Smile, smile);
        character.SetBlendshapeValue(CharacterBlendShapeType.Sadness, sad);
        character.SetBlendshapeValue(CharacterBlendShapeType.Surprise, surprise);
        character.SetBlendshapeValue(CharacterBlendShapeType.Thoughtful, thoughtful);
        character.SetBlendshapeValue(CharacterBlendShapeType.Angry, anger);
    }
}
