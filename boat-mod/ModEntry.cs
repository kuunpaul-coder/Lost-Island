using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace LostIslandBoat
{
    internal sealed class ModEntry : Mod
    {
        private const string IslandName = "Strict.LostIsland_MainIsland";
        private const int BeachX = 88;
        private const int BeachY = 20;
        private const int IslandX = 24;
        private const int IslandY = 28;
        private Texture2D boatTexture;

        public override void Entry(IModHelper helper)
        {
            boatTexture = helper.ModContent.Load<Texture2D>("assets/boat.png");
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Display.RenderedWorld += OnRenderedWorld;
            Monitor.Log("Lost Island boat and route menu loaded.", LogLevel.Info);
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || e.Button != SButton.Action)
                return;

            var location = Game1.currentLocation;
            var tile = e.Cursor.GrabTile;
            var playerTile = Game1.player.TilePoint;

            if (location.Name == "Beach" && IsNear(playerTile, BeachX, BeachY))
            {
                ShowBeachRoutes(location);
                Helper.Input.Suppress(e.Button);
            }
            else if (location.Name == IslandName && IsNear(playerTile, IslandX, IslandY))
            {
                ShowIslandRoutes(location);
                Helper.Input.Suppress(e.Button);
            }
        }

        private static bool IsNear(Point player, int x, int y)
        {
            return Math.Abs(player.X - x) <= 2 && Math.Abs(player.Y - y) <= 2;
        }

        private void ShowBeachRoutes(GameLocation location)
        {
            var responses = new[]
            {
                new Response("island", "На Затерянный остров"),
                new Response("cancel", "Отмена")
            };
            location.createQuestionDialogue("Куда отправимся?", responses, OnBeachRouteChosen);
        }

        private void OnBeachRouteChosen(Farmer who, string answer)
        {
            if (answer == "island")
                Game1.warpFarmer(IslandName, IslandX, IslandY, false);
        }

        private void ShowIslandRoutes(GameLocation location)
        {
            var responses = new[]
            {
                new Response("beach", "Вернуться на пляж"),
                new Response("cancel", "Остаться на острове")
            };
            location.createQuestionDialogue("Куда отправимся?", responses, OnIslandRouteChosen);
        }

        private void OnIslandRouteChosen(Farmer who, string answer)
        {
            if (answer == "beach")
                Game1.warpFarmer("Beach", BeachX, BEACH_RETURN_Y, false);
        }

        private const int BEACH_RETURN_Y = 21;

        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            string locationName = Game1.currentLocation.Name;
            if (locationName != "Beach" && locationName != IslandName)
                return;

            Vector2 tile = locationName == "Beach"
                ? new Vector2(BeachX, BeachY - 2)
                : new Vector2(IslandX, IslandY - 2);

            Vector2 worldPosition = tile * 64f;
            Vector2 screenPosition = Game1.GlobalToLocal(Game1.viewport, worldPosition);
            e.SpriteBatch.Draw(boatTexture, screenPosition, null, Color.White, 0f,
                Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
        }
    }
}