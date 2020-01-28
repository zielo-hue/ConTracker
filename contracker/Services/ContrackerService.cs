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

        public bool IsRegistered(Snowflake discordId)
        {
            try
            {
                _ = Contracker.GetPlayer(discordId: discordId.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}