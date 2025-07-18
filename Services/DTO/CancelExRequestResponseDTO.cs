using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CancelExRequestResponseDTO
    {
        public int Id { get; set; } // ID của ExaminationRequest

        public int UserId { get; set; } // ID của người dùng

        public string UserName { get; set; } // Tên người dùng

        public int ServiceId { get; set; } // ID của dịch vụ

        public string ServiceName { get; set; } // Tên dịch vụ

        public decimal ServicePrice { get; set; } // Giá dịch vụ

        public int SampleMethodId { get; set; } // ID của phương pháp lấy mẫu

        public string SampleMethodName { get; set; } // Tên phương pháp lấy mẫu

        public string StatusId { get; set; } // ID trạng thái (ví dụ: "6" cho Cancelled)

        public string StatusName { get; set; } // Tên trạng thái (ví dụ: "Cancelled")

        public DateTime AppointmentTime { get; set; } // Thời gian hẹn

        public DateTime? UpdateAt { get; set; } // Thời gian cập nhật (nullable)

        public int? StaffId { get; set; } // ID của nhân viên (nullable)
    }
}
