using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
	private StudioEventEmitter emitter;

	// (Optional) Prevent non-singleton constructor use.
	protected MusicManager() { }

	private void Start()
	{
		this.emitter = this.GetComponent<StudioEventEmitter>();
	}

	public void UpdateLayerMusic(int layer)
	{
		this.emitter.SetParameter("State", layer);
	}
}
