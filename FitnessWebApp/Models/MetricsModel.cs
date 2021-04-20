using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class MetricsModel
    {
        [Display(Name = "Возраст")]
        public int MetricAge { get; set; }

        [Display(Name = "Цель")]
        public string MetricGoal { get; set; }

        [Display(Name = "Проблемы со здоровьем")]
        public IList<string> MetricHealth { get; set; }

        [Display(Name = "Рост")]
        public int MetricHeight { get; set; }

        [Display(Name = "Вес")]
        public int MetricPullUps { get; set; }

        [Display(Name = "Вес")]
        public int MetricPushUps { get; set; }

        [Display(Name = "Вес")]
        public int MetricWeight { get; set; }

    }
}
