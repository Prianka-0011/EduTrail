import { Component } from '@angular/core';
import { EnrolementService } from '../services/enrolement.service';

@Component({
  selector: 'app-create-bulk-enrollments',
  templateUrl: './create-bulk-enrollments.component.html',
  styleUrl: './create-bulk-enrollments.component.scss'
})
export class CreateBulkEnrollmentsComponent {

  selectedFile: File | null = null;
  courseOfferingId = '';

  constructor(private enrollmentService: EnrolementService) {}

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      this.selectedFile = null;
      return;
    }

    const file = input.files[0];

    if (file.name.toLowerCase().endsWith('.csv')) {
      this.selectedFile = file;
    } else {
      alert('Only CSV files are allowed');
      this.selectedFile = null;
      input.value = '';
    }
  }

  onSubmit(): void {
    if (!this.selectedFile || !this.courseOfferingId) {
      alert('File and Course Offering are required');
      return;
    }

    this.enrollmentService
      .bulkCreateEnrollments(this.selectedFile, this.courseOfferingId)
      .subscribe({
        next: (response) => {
          console.log('Bulk upload success', response);
          this.resetForm();
        },
        error: (error) => {
          console.error('Bulk upload failed', error);
        }
      });
  }

  onCancel(): void {
    this.resetForm();
  }

  private resetForm(): void {
    this.selectedFile = null;
  }
}
