using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
  public static SoundEffects Instance;
  public AudioClip JumpingSound;
  public AudioClip playerShotSound;
  public AudioClip playerTakeDamage;
  public AudioClip playerSwordSound1;
  public AudioClip playerSwordSound2;
  public AudioClip pickaxe;
  public AudioClip playerDeath;
  // Awake is called before the start
  void Awake()
  {
    if (Instance != null)
    {
      Debug.LogError("D'autres instances de SoundEffects existent!");
    }
      Instance = this;
  }
  //Lorsque le joueur cours
  public void MakeJumpingSound()
  {
    MakeSound(JumpingSound);
  }
  public void MakeShootSound()
  {
    MakeSound(playerShotSound);
  }
  public void PlayerTakeDamageSound()
  {
    MakeSound(playerTakeDamage);
  }
  public void MakeSwordSound1()
  {
    MakeSound(playerSwordSound1);
  }
  public void MakeSwordSound2()
  {
    MakeSound(playerSwordSound2);
  }
    public void MakePickaxeSound()
  {
    MakeSound(pickaxe);
  }
    public void MakePlayerDeathSound()
  {
    MakeSound(playerDeath);
  }
  private void MakeSound(AudioClip chooseClip)
  {
    AudioSource.PlayClipAtPoint(chooseClip, transform.position);
  }
}
