import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { ActivatedRoute } from '@angular/router';
import { UserDashboardService } from '../../../services/user-dashboard.service';
import { ToastrService } from 'ngx-toastr';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-ta-lab-schedule',
  imports: [
    CommonModule,
    FullCalendarModule,
    MatIconModule
  ],
  templateUrl: './ta-lab-schedule.component.html',
  styleUrls: ['./ta-lab-schedule.component.scss']
})
export class TaLabScheduleComponent implements OnInit {

  EMPTY_ID = '00000000-0000-0000-0000-000000000000';

  calendarOptions: any = {
    plugins: [
      dayGridPlugin,
      timeGridPlugin,
      interactionPlugin
    ],

    initialView: 'dayGridMonth',
    timeZone: 'local',

    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek'
    },

    height: 'auto',

    dayMaxEvents: true,
    displayEventTime: true,
    eventDisplay: 'block',

    eventTimeFormat: {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true
    },

    // Week view settings
    slotMinTime: '06:00:00',
    slotMaxTime: '22:00:00',   // Optional: show until 10 PM
    scrollTime: '06:00:00',    // Scroll to 6 AM when opening week view
    allDaySlot: false,         // Optional: hide "All Day" row
    expandRows: true,
    nowIndicator: true,

    events: []
  };


  constructor(
    private enrollmentService: UserDashboardService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }


  ngOnInit(): void {
    this.loadTaschedule();
  }


  loadTaschedule(): void {

    const courseOfferingId =
      this.route.parent?.snapshot.paramMap.get(
        'courseOfferingId'
      ) ?? this.EMPTY_ID;


    this.enrollmentService
      .getTAAndLabHoursByCourseOffering(courseOfferingId)
      .subscribe({

        next: (data: any) => {

          const events: any[] = [];


          data?.detailsListDto?.forEach((enrollment: any) => {

            enrollment?.months?.forEach((month: any) => {

              month?.weeks?.forEach((week: any) => {

                week?.days?.forEach((day: any) => {


                  if (!day.labDate) {
                    return;
                  }


                  const date =
                    new Date(day.labDate);


                  const year =
                    date.getUTCFullYear();

                  const monthIndex =
                    date.getUTCMonth();

                  const dayNumber =
                    date.getUTCDate();



                  day?.slots?.forEach((slot: any) => {


                    if (
                      !slot.startTime ||
                      !slot.endTime
                    ) {
                      return;
                    }


                    const startParts =
                      slot.startTime.split(':');

                    const endParts =
                      slot.endTime.split(':');



                    const start =
                      new Date(
                        year,
                        monthIndex,
                        dayNumber,
                        Number(startParts[0]),
                        Number(startParts[1])
                      );


                    const end =
                      new Date(
                        year,
                        monthIndex,
                        dayNumber,
                        Number(endParts[0]),
                        Number(endParts[1])
                      );


                    events.push({

                      title:
                        `${enrollment.userName}'s Lab Hour`,

                      start,
                      end

                    });


                  });

                });

              });

            });

          });


          this.calendarOptions = {
            ...this.calendarOptions,
            events
          };


        },

        error: () => {

          this.toastr.error(
            'Failed to load TA lab schedule'
          );

        }

      });

  }

}