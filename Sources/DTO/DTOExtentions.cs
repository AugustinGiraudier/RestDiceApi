using ModelAppLib;

namespace DTO
{
    public static class DTOExtentions
    {

        public static Dice ToModel(this DiceDTO dto)
        {
            // TODO : ajouter les classes dto pour gérer les sides et na pas laisser null
            var ds = new Dice(new SecureRandomizer(), null);
            ds.Id = dto.ID;
            return ds;
        }

        // ...
    }
}
