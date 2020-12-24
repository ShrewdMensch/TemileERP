using System.Threading.Tasks;
using Utility.Notifications;

namespace Utility
{
    public interface ISend
    {
        Task<bool> Mail(EmailMessage message);
    }
}
