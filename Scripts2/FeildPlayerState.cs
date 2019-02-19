using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドでの位置や状態を保持し戦闘後に元に戻れるようにするためのクラス
/// </summary>

public class FeildPlayerState : MonoBehaviour {

    public static Vector3 feildPlayerPos;

	public static bool TelepoMode = false;

    public static bool rotButton = false;

    public static bool miniMap = false;
}
