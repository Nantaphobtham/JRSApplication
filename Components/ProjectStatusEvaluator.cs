using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public static class ProjectStatusEvaluator
    {
        public static string GetProjectStatus(List<PhaseWorking> phaseWorkings, int totalPhases)
        {
            // เตรียม Dictionary สำหรับเก็บสถานะของแต่ละเฟส (โดย key = phase_no)
            Dictionary<int, List<string>> phaseStatusMap = new Dictionary<int, List<string>>();

            foreach (var work in phaseWorkings)
            {
                if (!phaseStatusMap.ContainsKey(work.PhaseNo))
                    phaseStatusMap[work.PhaseNo] = new List<string>();

                phaseStatusMap[work.PhaseNo].Add(work.WorkStatus);
            }

            // 👇 ตัดสินสถานะของแต่ละเฟส
            Dictionary<int, string> effectivePhaseStatus = new Dictionary<int, string>();

            for (int phase = 1; phase <= totalPhases; phase++)
            {
                if (!phaseStatusMap.ContainsKey(phase))
                {
                    effectivePhaseStatus[phase] = WorkStatus.NotStarted;
                    continue;
                }

                List<string> statuses = phaseStatusMap[phase];

                if (statuses.Contains(WorkStatus.InProgress))
                {
                    effectivePhaseStatus[phase] = WorkStatus.InProgress;
                }
                else if (statuses.Contains(WorkStatus.Completed) && statuses.Contains(WorkStatus.Paid))
                {
                    effectivePhaseStatus[phase] = "CompletedAndPaid";
                }
                else if (statuses.Contains(WorkStatus.Completed) && statuses.Contains(WorkStatus.WaitingForInvoice))
                {
                    effectivePhaseStatus[phase] = "CompletedWaitingForInvoice";
                }
                else if (statuses.Contains(WorkStatus.Completed))
                {
                    effectivePhaseStatus[phase] = WorkStatus.Completed;
                }
                else
                {
                    effectivePhaseStatus[phase] = WorkStatus.NotStarted;
                }
            }

            // ✅ LOGIC PROJECT STATUS
            bool allNotStarted = effectivePhaseStatus.Values.All(s => s == WorkStatus.NotStarted);
            bool allCompletedPaid = effectivePhaseStatus.Values.All(s => s == "CompletedAndPaid");
            bool anyInProgress = effectivePhaseStatus.Values.Any(s => s == WorkStatus.InProgress || s == "CompletedWaitingForInvoice");

            if (allNotStarted)
                return WorkStatus.NotStarted;

            if (allCompletedPaid)
                return "Completed";

            if (anyInProgress)
                return WorkStatus.InProgress;

            return "Unknown"; // fallback ป้องกันหลุด logic
        }
    }
}
