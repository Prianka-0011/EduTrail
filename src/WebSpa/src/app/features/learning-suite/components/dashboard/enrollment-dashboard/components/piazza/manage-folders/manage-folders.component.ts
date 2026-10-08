import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ManageFolderService } from '../services/manage-folder.service';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IFolder, IFolderDetail } from '../interfaces/IFolder';
import { ConfirmDialogComponent } from '../../../../../../../../shared/components/confirm-dialog/confirm-dialog.component';


@Component({
  selector: 'app-manage-folders',
  standalone: true,
  imports: [CommonModule, FormsModule, ConfirmDialogComponent],
  templateUrl: './manage-folders.component.html',
  styleUrl: './manage-folders.component.scss'
})
export class ManageFoldersComponent implements OnInit {

  folders: IFolderDetail[] = [];
  newfolders: IFolderDetail[] = [];
  isSubFolderCreation?: boolean = false;
  selectedFolders: string[] = [];
  showDeleteModal = false;
  expandAll = false;

  newFolderName = '';
  isNumberedFolder = false;
  startNumber = 1;
  endNumber = 4;

  courseOfferingId = '';

  constructor(
    private manageFolderService: ManageFolderService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.courseOfferingId = this.route.parent?.snapshot.paramMap.get('courseOfferingId') ?? "";
    this.getFolders();
  }

  getFolders(): void {
    this.manageFolderService.getFolders(this.courseOfferingId).subscribe({
      next: (response: any) => {

        const folders = response?.detailsListDto ?? [];
        this.folders = folders.map((folder: any) =>
          this.mapFolderFromApi(folder)
        );
      },
      error: (error) => {
        this.toastr.error(error)
      }
    });
  }

  getSubFolders(parentId: string, parentFolder: IFolderDetail): void {
    this.manageFolderService.getSubFolders(parentId).subscribe({
      next: (response: any) => {
        console.log(response, "Subfolder")
        const folders = response?.detailsListDto ?? [];
        parentFolder.subFolders = folders.map((folder: any) =>
          this.mapFolderFromApi(folder)
        );
      },
      error: (error) => {
        this.toastr.error(error)
      }
    });
  }

  addFolder(): void {

    if (!this.newFolderName.trim()) {
      return;
    }

    if (this.isNumberedFolder) {

      const newFolders: IFolderDetail[] = [];

      for (let i = this.startNumber; i <= this.endNumber; i++) {

        newFolders.push({
          id: crypto.randomUUID(),
          name: `${this.newFolderName}-${i}`,
          displayOrder: i,
          isActive: true,
          courseOfferingId: this.courseOfferingId,
          expanded: false,
          showCreateSubFolder: false,
          subFolders: []
        });
      }
      this.newfolders = newFolders;
    } else {

      this.newfolders = [
        {
          id: crypto.randomUUID(),
          name: this.newFolderName,
          expanded: true,
          showCreateSubFolder: false,
          subFolders: []
        }
      ];
    }

    this.newFolderName = '';

    this.saveFolders();
  }

  toggleFolder(folder: IFolderDetail): void {
    folder.expanded = !folder.expanded;
  }

  toggleAllFolders(toggleAllFolders: IFolderDetail): void {
    toggleAllFolders.expanded = !toggleAllFolders.expanded;
    this.getSubFolders(toggleAllFolders.id, toggleAllFolders);
  }

  createSubFolder(folder: IFolderDetail): void {
    folder.showCreateSubFolder = true;
    folder.subFolderStart = 1;
    folder.subFolderEnd = 4;
  }

  cancelSubFolder(folder: IFolderDetail): void {
    folder.showCreateSubFolder = false;
    folder.subFolderName = '';
    folder.subFolderStart = undefined;
    folder.subFolderEnd = undefined;
    folder.isNumberedSubFolder = false;
  }

  closeAllPanels(folders: IFolderDetail[]): void {

    folders.forEach(folder => {

      folder.showCreateSubFolder = false;

      if (folder.subFolders?.length) {
        this.closeAllPanels(folder.subFolders);
      }

    });
  }

  cancelCreateSubFolder(folder: IFolderDetail): void {
    folder.showCreateSubFolder = false;
  }

  editFolder(folder: IFolderDetail): void {
    folder.isEditing = true;
    folder.editName = folder.name
  }

  saveEdit(folder: IFolderDetail): void {
    if (!folder.editName?.trim()) return;
    console.log("folder Prianka", folder.displayOrder)
    const payload = {
      folder: {
        id: folder.id,
        name: folder.editName,
        displayOrder: folder.displayOrder,
        parentFolderId: folder.parentFolderId,
        courseOfferingId: this.courseOfferingId
      }
    };

    this.manageFolderService.updateFolder(folder.id, payload).subscribe({
      next: () => {
        folder.name = folder.editName!;
        folder.isEditing = false;

        this.toastr.success('Folder updated successfully');
      },
      error: (err) => {
        this.toastr.error('Update failed');
        console.error(err);
      }
    });
  }

  cancelEdit(folder: IFolderDetail): void {
    folder.isEditing = false;
  }

  onConfirmDelete(): void {
    this.manageFolderService.deleteFolders(this.selectedFolders).subscribe({
      next: () => {
        this.toastr.success('Folder(s) deleted successfully');
        this.selectedFolders = [];
        this.getFolders();
        this.showDeleteModal = false;
      },
      error: () => {
        this.toastr.error('Delete failed');
        this.showDeleteModal = false;
      }
    });
  }

  onCancelDelete(): void {
    this.showDeleteModal = false;
  }

  deleteSelected(): void {
    if (!this.selectedFolders.length) {
      return;
    }

    this.showDeleteModal = true;
  }

  onSelect(folder: IFolderDetail, event: Event): void {

    const checked = (event.target as HTMLInputElement).checked;

    if (checked) {

      if (!this.selectedFolders.includes(folder.id)) {
        this.selectedFolders.push(folder.id);
      }

    } else {

      this.selectedFolders =
        this.selectedFolders.filter(id => id !== folder.id);
    }
  }

  editSubFolder(subFolder: IFolderDetail): void {
    subFolder.isEditing = true;
    subFolder.editName = subFolder.name;
  }

  cancelSubFolderEdit(subFolder: IFolderDetail): void {
    subFolder.isEditing = false;
    subFolder.editName = subFolder.name;
  }

  saveSubFolder(subFolder: IFolderDetail): void {

    if (!subFolder.editName?.trim()) {
      return;
    }

    const payload = {
      folder: {
        id: subFolder.id,
        name: subFolder.editName,
        displayOrder: subFolder.displayOrder,
        parentFolderId: subFolder.parentFolderId,
        courseOfferingId: this.courseOfferingId
      }
    };

    this.manageFolderService.updateFolder(subFolder.id, payload).subscribe({
      next: () => {

        subFolder.name = subFolder.editName;
        subFolder.isEditing = false;

        this.toastr.success('Subfolder updated successfully');
      },
      error: () => {
        this.toastr.error('Update failed');
      }
    });

  }

  addSubFolder(parent: IFolderDetail): void {

    if (!parent.subFolderName?.trim()) {
      return;
    }

    const newSubFolders: IFolderDetail[] = [];

    if (parent.isNumberedSubFolder) {

      for (
        let i = parent.subFolderStart ?? 1;
        i <= (parent.subFolderEnd ?? 1);
        i++
      ) {
        newSubFolders.push({
          id: crypto.randomUUID(),
          name: `${parent.subFolderName}-${i}`,
          displayOrder: i,
          isActive: true,
          courseOfferingId: this.courseOfferingId,
          parentFolderId: parent.id,
          expanded: true,
          showCreateSubFolder: false,
          subFolders: []
        });
      }

    } else {

      newSubFolders.push({
        id: crypto.randomUUID(),
        name: parent.subFolderName,
        displayOrder: 1,
        isActive: true,
        courseOfferingId: this.courseOfferingId,
        parentFolderId: parent.id,
        expanded: true,
        showCreateSubFolder: false,
        subFolders: []
      });

    }

    parent.subFolderName = '';
    parent.isNumberedSubFolder = false;
    parent.subFolderStart = 1;
    parent.subFolderEnd = 4;
    parent.showCreateSubFolder = false;
    this.newfolders = newSubFolders;
    this.saveFolders();
  }

  saveFolders(): void {

    const payload = {
      folderDto: {
        detailsListDto: this.mapFoldersForApi(this.newfolders)
      }
    };

    this.manageFolderService.createFolders(payload).subscribe({
      next: () => {
        this.toastr.success("Folder save successfully")
        this.getFolders();
      },
      error: (error) => {
        this.toastr.error(error)
      }
    });
  }

  private mapFoldersForApi(folders: IFolderDetail[]): any[] {

    return folders.map((folder, index) => ({
      id: folder.id,
      name: folder.name,
      displayOrder: index + 1,
      isActive: true,
      courseOfferingId: this.courseOfferingId,
      parentFolderId: folder.parentFolderId ?? null,
    }));
  }

  private mapFolderFromApi(folder: any): IFolderDetail {

    return {
      id: folder.id,
      name: folder.name,
      expanded: false,
      showCreateSubFolder: false,
      displayOrder: folder.displayOrder,
      parentFolderId: folder.parentFolderId,
      courseOfferingId: folder.courseOfferingId
    };
  }
}