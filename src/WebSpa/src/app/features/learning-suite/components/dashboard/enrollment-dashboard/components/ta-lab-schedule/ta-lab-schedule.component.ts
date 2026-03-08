import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
@Component({
  selector: 'app-ta-lab-schedule',
  imports: [CommonModule, FullCalendarModule],
  templateUrl: './ta-lab-schedule.component.html',
  styleUrls: ['./ta-lab-schedule.component.scss']
})
export class TaLabScheduleComponent {
  calendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek'
    },
    dayMaxEvents: true,
    displayEventTime: true,
    height: 'auto',
    events: [
      {
        title: "Aidan's Lab Hour",
        start: '2026-03-03T11:30:00',
        end: '2026-03-03T12:30:00'
      },
      {
        title: "Faizan's Lab Hour",
        start: '2026-03-03T15:00:00',
        end: '2026-03-03T16:00:00'
      },
      {
        title: "CSE 100 PSS - DS",
        start: '2026-03-05T09:00:00',
        end: '2026-03-05T10:00:00'
      },
      {
        title: "Kaleigh's Lab Hour",
        start: '2026-03-06T10:30:00',
        end: '2026-03-06T11:30:00'
      },
      {
        title: "Kasem's Lab Hour",
        start: '2026-03-06T10:30:00',
        end: '2026-03-06T11:30:00'
      }
    ]
  };
}