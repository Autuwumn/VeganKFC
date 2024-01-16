using MapsExt;
using ModsPlus;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnboundLib.GameModes;
using UnityEngine;
using VKFC.Monos;
using UnboundLib.Networking;
using VKFC.UnessesaryAddons;
using UnboundLib;
using Photon.Pun.Simple;
using MapsExt.MapObjects;

namespace VKFC.Cards
{
    public class BoxedBullets : CustomEffectCard<BoxThoseBullets>
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Boxed Bullets",
            Description = "Shoot boxes at people instead of bullets",
            ModName = VKFC.ModInitials,
            Art = VKFC.ArtAssets.LoadAsset<GameObject>("C_BoxedBullets"),
            Rarity = CardInfo.Rarity.Rare,
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            OwnerOnly = true
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
    }
}
namespace VKFC.Monos
{
    public class BoxThoseBullets : CardEffect
    {
        public List<GameObject> Boxes = new List<GameObject>();
        int curBox = 0;
        protected override void OnDestroy()
        {
            KillBoxes();
        }
        public override void OnShoot(GameObject projectile)
        {
            if (Boxes.Count == 0) SpawnBoxes();
            var a = Boxes[curBox];
            curBox++;
            if (curBox == VKFC.BoxCap) curBox = 0;
            a.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            a.transform.position = projectile.transform.position + (player.data.aimDirection.normalized * 3);
            a.transform.rotation = Quaternion.identity;
            float pSpeedMult = Mathf.Sqrt(gun.projectileSpeed);
            var shootAngle = player.data.aimDirection.normalized;
            var speead = UnityEngine.Random.Range(0f - gun.spread, gun.spread);
            speead /= (1f + gun.projectileSpeed * 0.5f) * 0.5f;
            shootAngle += Vector3.Cross(shootAngle, Vector3.forward) * speead;
            a.GetComponent<Rigidbody2D>().AddForce(shootAngle.normalized * 40000000 * gun.damage * pSpeedMult);
            PhotonNetwork.Destroy(projectile);
        }
        [UnboundRPC]
        public static void RPCDistributeOwners(int playerId)
        {
            foreach (var o in FindObjectsOfType<OwnerScript>())
            {
                if (o.Owner == null)
                {
                    o.Owner = PlayerManager.instance.GetPlayerWithID(playerId);
                    return;
                }
            }
        }
        public override void OnTakeDamage(Vector2 damage, bool selfDamage)
        {
            if (player.data.health < 0)
            {
                ResetBoxes();
            }
        }
        public override IEnumerator OnPointEnd(IGameModeHandler gameModeHandler)
        {
            KillBoxes();
            yield return null;
        }
        public override IEnumerator OnPointStart(IGameModeHandler gameModeHandler)
        {
            SpawnBoxes();
            yield return null;
        }
        public void SpawnBoxes()
        {
            if(Boxes.Count > 0) KillBoxes();
            for(var i = 0; i < VKFC.BoxCap; i++)
            {
                var a = PhotonNetwork.Instantiate("VKFC_Box", new Vector3(999, 999, 0), Quaternion.identity);
                Boxes.Add(a);
                NetworkingManager.RPC(typeof(BoxThoseBullets), nameof(RPCDistributeOwners), player.playerID);
            }
        }
        public void ResetBoxes()
        {
            foreach(var b in Boxes)
            {
                b.transform.position = new Vector3(999, 999, 0);
            }
        }
        public void KillBoxes()
        {
            foreach (var b in Boxes)
            {
                PhotonNetwork.Destroy(b.gameObject);
            }
            Boxes.Clear();
        }
    }
}
