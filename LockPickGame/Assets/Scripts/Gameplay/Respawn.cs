using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    [SerializeField]
    private Transform m_respawnLocation;
    [SerializeField]
    private Transform m_deathZone;

    private PlayerRaycast m_player;

    void Start()
    {
        m_player = GetComponent<PlayerRaycast>();
    }

	void Update ()
    {
        if (m_player != null)
        {
            if (m_player.transform.position.y < m_deathZone.position.y)
            {
                m_player.transform.position = m_respawnLocation.position;
            }
        }
	}
}
