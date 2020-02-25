using System;
using contrackerNET;
using Disqord;

namespace contracker.Services
{
    public class ContrackerService
    {
        public ContrackerAPI Contracker;

        public ContrackerService(string key)
        {
            Contracker = new ContrackerAPI(key);
        }

        public Player GetPlayer(string steamId = null, string discordId = null)
        {
            if (steamId == null && discordId == null)
            {
                throw new ArgumentNullException(nameof(steamId) + ", " + nameof(discordId));
            }

            // ContrackerAPI does not update, placeholder handler for 404s
            try
            {
                return Contracker.GetPlayer(steamId, discordId.ToString());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}