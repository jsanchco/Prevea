﻿namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class TrainingService
    {
        [ForeignKey("Simulator")]
        public int Id { get; set; }

        public string Observations { get; set; }

        public virtual Simulator Simulator { get; set; }
    }
}
