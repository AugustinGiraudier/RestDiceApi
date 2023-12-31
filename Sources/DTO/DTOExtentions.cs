﻿using ModelAppLib;

namespace DTO
{
    public static class DTOExtentions
    {

        // ----------- TO MODEL ----------- //
        public static Dice ToModel(this DiceDTO dto, IDataManager manager)
        {
            DiceSideType[] dsts = new DiceSideType[dto.SideTypes.Count()];
            int cpt = 0;
            foreach(DiceSideTypeDTO dstdto in dto.SideTypes)
            {
                dsts[cpt] = dto.SideTypes[cpt].ToModel(manager);
                cpt++;
            }
            var ds = new Dice(new SecureRandomizer(), dsts);
            ds.Id = dto.ID;
            return ds;
        }
        public static Dice ToModel(this InputDiceDTO dto, IDataManager manager)
        {
            DiceSideType[] dsts = new DiceSideType[dto.SideTypes.Count()];
            int cpt = 0;
            foreach(DiceSideTypeDTO dstdto in dto.SideTypes)
            {
                dsts[cpt] = dto.SideTypes[cpt].ToModel(manager);
                cpt++;
            }
            var ds = new Dice(new SecureRandomizer(), dsts);
            ds.Id = 0;
            return ds;
        }

        public static DiceSideType ToModel(this DiceSideTypeDTO dto, IDataManager manager)
        {
            var dst = new DiceSideType(dto.nbPrototype, manager.GetDiceSideWithId(dto.prototypeId).Result);
            return dst;
        }

        public static DiceSide ToModel(this DiceSideDTO dto)
        {
            var ds = new DiceSide(dto.image);
            ds.Id = dto.ID;
            return ds;
        }
        public static DiceSide ToModel(this InputDiceSideDTO dto)
        {
            var ds = new DiceSide(dto.image);
            ds.Id = 1;
            return ds;
        }


        // ----------- TO DTO ----------- //
        public static DiceDTO ToDTO(this Dice model)
        {
            DiceSideTypeDTO[] dsts = new DiceSideTypeDTO[model.SideTypes.Count()];
            int cpt = 0;
            foreach (DiceSideType dstdto in model.SideTypes)
            {
                dsts[cpt] = model.SideTypes[cpt].ToDTO();
                cpt++;
            }
            var dto = new DiceDTO();
            dto.ID = model.Id;
            dto.SideTypes = dsts;
            return dto;
        }
        public static DiceSideTypeDTO ToDTO(this DiceSideType model)
        {
            var dto = new DiceSideTypeDTO();
            dto.nbPrototype = model.NbSide;
            dto.prototypeId = (int)model.Prototype.Id;
            return dto;
        }
        public static DiceSideDTO ToDTO(this DiceSide model)
        {
            var dto = new DiceSideDTO();
            dto.image = model.Image;
            dto.ID = model.Id;
            return dto;
        }

        public static IEnumerable<DiceDTO> ToDTO(this IEnumerable<Dice> model)
        {
            var dto = new List<DiceDTO>();
            foreach(Dice dice in model)
            {
                dto.Add(dice.ToDTO());
            }
            return dto;
        }
        public static IEnumerable<DiceSideDTO> ToDTO(this IEnumerable<DiceSide> model)
        {
            var dto = new List<DiceSideDTO>();
            foreach (DiceSide diceside in model)
            {
                dto.Add(diceside.ToDTO());
            }
            return dto;
        }

    }
}
