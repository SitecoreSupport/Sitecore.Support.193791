using System;
using System.Collections.Generic;
using Sitecore.Analytics.Aggregation.Pipeline;
using Sitecore.Analytics.Model;
using Sitecore.ContentTesting.Analytics.Aggregation.Data.Model.Facts;
using Sitecore.ContentTesting.Configuration;
using Sitecore.Diagnostics;

namespace Sitecore.Support.ContentTesting.Analytics.Aggregation.Pipeline
{
  public class PersonalizationRulesExposure : Sitecore.ContentTesting.Analytics.Aggregation.Pipeline.PersonalizationRulesExposure
  {
    protected override void OnProcess(AggregationPipelineArgs args)
    {
      if (Settings.IsAutomaticContentTestingEnabled)
      {
        Assert.ArgumentNotNull(args, "args");
        VisitData visit = args.Context.Visit;
        if (((visit != null) && (visit.Pages != null)) && (visit.Pages.Count > 0))
        {
          RulesExposure fact = args.GetFact<RulesExposure>();
          Dictionary<Guid, List<KeyValuePair<Guid, Guid>>> pages = new Dictionary<Guid, List<KeyValuePair<Guid, Guid>>>();
          foreach (PageData data2 in visit.Pages)
          {
            if (data2.PersonalizationData != null)
            {
              if (!pages.ContainsKey(data2.Item.Id))
              {
                pages.Add(data2.Item.Id, new List<KeyValuePair<Guid, Guid>>());
              }
              List<KeyValuePair<Guid, Guid>> list = pages[data2.Item.Id];
              foreach (PersonalizationRuleData data3 in data2.PersonalizationData.ExposedRules)
              {
                RulesExposureKey key = this.GetKey(data2, data3);
                RulesExposureValue value2 = this.GetValue(data2, data3, visit, pages);
                fact.Emit(key, value2);
                KeyValuePair<Guid, Guid> item = new KeyValuePair<Guid, Guid>(data3.RuleSetId.ToGuid(), data3.RuleId.ToGuid());
                list.Add(item);
              }
            }
          }
        }
      }
    }
  }
}
