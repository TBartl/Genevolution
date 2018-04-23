using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource die;
	public AudioSource tick;

	public AudioSource nestNew;
	public AudioSource nestVisited;
	public AudioSource eggCrack;

	public AudioSource jump;
	public AudioSource wallJump;
	public AudioSource airJump;

	public AudioSource land;
	public AudioSource footstep;

	public AudioSource music;

	public static SoundManager S;

	void Awake() {
		S = this;
	}

}
