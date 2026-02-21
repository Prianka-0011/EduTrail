import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IUserDetail } from '../interfaces/iUserDetail';
import { UserService } from '../services/user.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { IDropdownItem } from '../../../../../shared/interface/iDropdownItem';
import { Subject } from 'rxjs/internal/Subject';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { IUser } from '../interfaces/iUser';

const EMPTY_ID = '00000000-0000-0000-0000-000000000000';
@Component({
  selector: 'app-user-create-or-edit',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-create-or-edit.component.html',
  styleUrl: './user-create-or-edit.component.scss'
})
export class UserCreateOrEditComponent implements OnInit, OnChanges {

  @Input() resetTrigger = false;
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  @ViewChild('userForm') userForm!: NgForm;

  user: IUserDetail = this.getEmptyUser();

  allRoles: IDropdownItem[] = [];
  roles: IDropdownItem[] = [];
  selectedRoles: IDropdownItem[] = [];

  roleSearch$ = new Subject<string>();

  isFirstNameFocused = false;
  isMiddleNameFocused = false;
  isLastNameFocused = false;
  isEmailFocused = false;
  isPasswordFocused = false;

  constructor(
    private service: UserService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.loadUser();

    this.roleSearch$
      .pipe(debounceTime(200))
      .subscribe(term => {
        const search = term.toLowerCase();

        this.roles = this.allRoles
          .filter(r => r.name.toLowerCase().includes(search))
          .filter(r => !this.selectedRoles.some(s => s.id === r.id));
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['resetTrigger']?.currentValue && this.userForm) {
      this.resetForm();
    }
  }

  loadUser(): void {
    const userId = this.route.snapshot.queryParamMap.get('id') ?? EMPTY_ID;

    this.service.getById(userId).subscribe((data: IUser) => {
      this.user = { ...data.detailDto };

      this.allRoles = data.dropdownRoleList ?? [];
      this.selectedRoles = data.detailDto.selectedRoleList ?? [];

      this.roles = this.allRoles.filter(
        r => !this.selectedRoles.some(s => s.id === r.id)
      );
    });
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    this.user.selectedRoleList = [...this.selectedRoles];
    console.log(this.user, "User to submit with selected roles");
    const request$ =
      this.user.id === EMPTY_ID
        ? this.service.create(this.user)
        : this.service.update(this.user);

    request$.subscribe(() => {
      this.saved.emit();
      this.resetForm();
    });
  }

  cancelForm(): void {
    this.resetForm();
    this.cancel.emit();
  }

  resetForm(): void {
    this.user = this.getEmptyUser();
    this.selectedRoles = [];
    this.roles = [...this.allRoles];
    this.userForm.resetForm(this.user);
  }

  getEmptyUser(): IUserDetail {
    return {
      id: EMPTY_ID,
      firstName: '',
      middleName: '',
      lastName: '',
      email: '',
      password: '',
      isActive: true,
      selectedRoleList: []
    };
  }

  selectRole(role: IDropdownItem): void {
    if (this.selectedRoles.some(r => r.id === role.id)) return;

    this.selectedRoles.push(role);
    this.roles = this.roles.filter(r => r.id !== role.id);
  }

  removeRole(role: IDropdownItem): void {
    this.selectedRoles = this.selectedRoles.filter(r => r.id !== role.id);
    this.roles.push(role);
  }
}