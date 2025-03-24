using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Observable<T>
{
    [SerializeField] private T currentValue;

    public Action<T, T> OnValueChanged;

    public Observable(T value)
    {
        currentValue = value;
    }

    public T Value
    {
        get => currentValue;

        set
        {
            var oldValue = currentValue;
            currentValue = value;
            Invoke(oldValue, currentValue);
        }
    }

    private void Invoke(T oldValue, T newValue)
    {
        OnValueChanged?.Invoke(oldValue, newValue);
    }

    public static implicit operator T(Observable<T> observable)
    {
        return observable.currentValue;
    }
}

#if UNITY_EDITOR
// This is responsible for how Observable is displayed in the inspector
[CustomPropertyDrawer(typeof(Observable<>))]
public class ObservablePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property.FindPropertyRelative("currentValue"), label);

        if (EditorGUI.EndChangeCheck())
        {
            var oldValue = property.FindPropertyRelative("currentValue").GetUnderlyingValue();
            property.serializedObject.ApplyModifiedProperties();
            var newValue = property.FindPropertyRelative("currentValue").GetUnderlyingValue();

            if (!oldValue.Equals(newValue))
            {
                var targetObject = property.serializedObject.targetObject;
                var field = fieldInfo;
                var observableInstance = field.GetValue(targetObject);

                var invokeMethod = observableInstance
                    .GetType()
                    .GetMethod("Invoke", BindingFlags.NonPublic | BindingFlags.Instance);

                if (invokeMethod != null) invokeMethod.Invoke(observableInstance, new[] { oldValue, newValue });
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif