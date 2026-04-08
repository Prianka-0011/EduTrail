import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { ActivatedRoute } from '@angular/router';
import { UserDashboardService } from '../../../services/user-dashboard.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ta-lab-schedule',
  imports: [CommonModule, FullCalendarModule],
  templateUrl: './ta-lab-schedule.component.html',
  styleUrls: ['./ta-lab-schedule.component.scss']
})
export class TaLabScheduleComponent implements OnInit {

  EMPTY_ID = '00000000-0000-0000-0000-000000000000';

  // calendarOptions: any = {
  //   plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
  //   initialView: 'dayGridMonth',
  //   timeZone: 'local',
  //   headerToolbar: {
  //     left: 'prev,next today',
  //     center: 'title',
  //     right: 'dayGridMonth,timeGridWeek'
  //   },
  //   dayMaxEvents: true,
  //   displayEventTime: true,
  //   height: 'auto',
  //   events: []
  // };
  calendarOptions: any = {
  plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
  initialView: 'dayGridMonth',
  timeZone: 'local',
  headerToolbar: {
    left: 'prev,next today',
    center: 'title',
    right: 'dayGridMonth,timeGridWeek'
  },
  dayMaxEvents: true,
  displayEventTime: true,
  height: 'auto',

  eventDisplay: 'block',
  eventTimeFormat: {
    hour: '2-digit',
    minute: '2-digit',
    hour12: true
  },

  dayHeaderClassNames: 'fc-custom-header',
  dayCellClassNames: 'fc-custom-day',
  eventClassNames: 'fc-custom-event',

  events: []
};

  constructor(
    private enrolementService: UserDashboardService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadTaschedule();
  }

  loadTaschedule() {

    const courseOfferingId = this.route.parent?.snapshot.paramMap.get('courseOfferingId');

    this.enrolementService
      .getTAAndLabHoursByCourseOffering(courseOfferingId ?? this.EMPTY_ID)
      .subscribe({
        next: (data: any) => {

          const events: any[] = [];

          data?.detailsListDto?.forEach((enrollment: any) => {

            enrollment?.months?.forEach((month: any) => {

              month?.weeks?.forEach((week: any) => {

                week?.days?.forEach((day: any) => {

                  if (!day.labDate) return;

                  const date = new Date(day.labDate);

                  const year = date.getUTCFullYear();
                  const monthIndex = date.getUTCMonth();
                  const dayNumber = date.getUTCDate();

                  day?.slots?.forEach((slot: any) => {

                    if (!slot.startTime || !slot.endTime) return;

                    const startParts = slot.startTime.split(':');
                    const endParts = slot.endTime.split(':');

                    const start = new Date(
                      year,
                      monthIndex,
                      dayNumber,
                      +startParts[0],
                      +startParts[1]
                    );

                    const end = new Date(
                      year,
                      monthIndex,
                      dayNumber,
                      +endParts[0],
                      +endParts[1]
                    );

                    events.push({
                      title: `${enrollment.userName}'s Lab Hour`,
                      start: start,
                      end: end
                    });

                  });

                });

              });

            });

          });

          this.calendarOptions = {
            ...this.calendarOptions,
            events: events
          };

        },
        error: () => {
          this.toastr.error('Failed to load TA lab schedule');
        }
      });
  }

}
