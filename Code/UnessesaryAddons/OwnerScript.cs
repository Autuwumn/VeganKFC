using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace VKFC.UnessesaryAddons
{
    public class OwnerScript : MonoBehaviour
    {
        public Player Owner = null;
        private Rigidbody2D rb;

        public void FixedUpdate()
        {
            if(Owner != null)
            {
                rb = GetComponent<Rigidbody2D>();
                rb.mass = Owner.data.weaponHandler.gun.damage*20000;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = PlayerManager.instance.GetColorFromPlayer(Owner.playerID).color;
                GetComponent<SFPolygon>().shadowLayers = -1;
            }
        }
    }
}
