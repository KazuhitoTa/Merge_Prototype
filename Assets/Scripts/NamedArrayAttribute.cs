using UnityEngine;

/// <summary>
/// インスペクター配列内要素名変更
/// </summary>
public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}