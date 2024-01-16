using ModsPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.GameModes;
using VKFC.Monos;
using VKFC.Cards;
using UnityEngine;

namespace VKFC.Cards
{
    public class StayWinning : CustomEffectCard<WeStayWinning>
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "We Stay Winning",
            Description = "Insane stats, but don't lose",
            ModName = VKFC.ModInitials,
            Art = VKFC.ArtAssets.LoadAsset<GameObject>("C_WeStayWinning"),
            Rarity = CardInfo.Rarity.Rare,
            Theme = CardThemeColor.CardThemeColorType.FirepowerYellow,
            Stats = new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    amount = "+150%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage",
                },
                new CardInfoStat()
                {
                    amount = "+150%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Attack Speed",
                },
                new CardInfoStat()
                {
                    amount = "+150%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Reload Time",
                },
                new CardInfoStat()
                {
                    amount = "+150%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bullet Speed",
                },
                new CardInfoStat()
                {
                    amount = "+15",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Ammo",
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            gun.damage = 2.5f;
            gun.attackSpeed = 1 / 2.5f;
            gun.reloadTime = 1 / 2.5f;
            gun.projectileSpeed = 2.5f;
            gun.ammo = 15;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            VKFC.instance.ExecuteAfterFrames(20, () =>
            {
                foreach(var players in PlayerManager.instance.players)
                {
                    player.GetComponentInChildren<WeStayWinning>().TeamScore.Add(players.teamID, GameModeManager.CurrentHandler.GetTeamScore(players.teamID).rounds);
                }
            });
        }
    }
}
namespace VKFC.Monos
{
    public class WeStayWinning : CardEffect
    {
        public Dictionary<int, int> TeamScore = new Dictionary<int, int>();

        public override IEnumerator OnPickPhaseStart(IGameModeHandler gameModeHandler)
        {
            foreach(var ts in TeamScore)
            {
                if(ts.Key != player.teamID && GameModeManager.CurrentHandler.GetTeamScore(ts.Key).rounds != ts.Value)
                {
                    ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
                    while (player.data.currentCards.IndexOf(StayWinning.card) != -1)
                    {
                        ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, player.data.currentCards.IndexOf(StayWinning.card));
                    }
                }
            }
            yield return null;
        }
    }
}