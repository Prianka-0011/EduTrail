import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

export interface Folder {
  id: string;
  name: string;
  expanded: boolean;
  showCreateSubFolder: boolean;
  subFolders: Folder[];

  subFolderName?: string;
  isNumberedSubFolder?: boolean;
  subFolderStart?: number;
  subFolderEnd?: number;
}

@Component({
  selector: 'app-manage-folders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-folders.component.html',
  styleUrl: './manage-folders.component.scss'
})
export class ManageFoldersComponent {

  folders: Folder[] = [];

  selectedFolders: string[] = [];

  expandAll = true;

  newFolderName = '';
  isNumberedFolder = false;
  startNumber = 1;
  endNumber = 4;

  addFolder(): void {

    if (!this.newFolderName.trim()) {
      return;
    }

    if (this.isNumberedFolder) {

      const newFolders: Folder[] = [];

      for (let i = this.startNumber; i <= this.endNumber; i++) {

        newFolders.push({
          id: crypto.randomUUID(),
          name: `${this.newFolderName}${i}`,
          expanded: true,
          showCreateSubFolder: false,
          subFolders: []
        });

      }

      this.folders = [...this.folders, ...newFolders];

    } else {

      this.folders = [
        ...this.folders,
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
  }

  toggleFolder(folder: Folder): void {
    folder.expanded = !folder.expanded;
  }

  toggleAllFolders(): void {

    this.expandAll = !this.expandAll;

    const update = (folders: Folder[]) => {

      folders.forEach(folder => {

        folder.expanded = this.expandAll;

        if (folder.subFolders.length) {
          update(folder.subFolders);
        }

      });

    };

    update(this.folders);
  }

  createSubFolder(folder: Folder): void {

    this.closeAllPanels(this.folders);

    folder.showCreateSubFolder = !folder.showCreateSubFolder;
  }

  closeAllPanels(folders: Folder[]): void {

    folders.forEach(folder => {

      folder.showCreateSubFolder = false;

      if (folder.subFolders.length) {
        this.closeAllPanels(folder.subFolders);
      }

    });
  }

  cancelCreateSubFolder(folder: Folder): void {
    folder.showCreateSubFolder = false;
  }

  editFolder(folder: Folder): void {
    console.log(folder);
  }

  deleteSelected(): void {

    this.folders = this.folders.filter(
      folder => !this.selectedFolders.includes(folder.id)
    );

    this.selectedFolders = [];
  }

  onSelect(folder: Folder, event: Event): void {

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

  addSubFolder(folder: Folder): void {

    if (!folder.subFolderName?.trim()) {
      return;
    }

    const newSubFolders: Folder[] = [];

    if (folder.isNumberedSubFolder) {

      for (
        let i = folder.subFolderStart ?? 1;
        i <= (folder.subFolderEnd ?? 1);
        i++
      ) {

        newSubFolders.push({
          id: crypto.randomUUID(),
          name: `${folder.subFolderName}${i}`,
          expanded: true,
          showCreateSubFolder: false,
          subFolders: [],
          subFolderName: '',
          isNumberedSubFolder: false,
          subFolderStart: 1,
          subFolderEnd: 4
        });

      }

    } else {

      newSubFolders.push({
        id: crypto.randomUUID(),
        name: folder.subFolderName,
        expanded: true,
        showCreateSubFolder: false,
        subFolders: [],
        subFolderName: '',
        isNumberedSubFolder: false,
        subFolderStart: 1,
        subFolderEnd: 4
      });

    }

    folder.subFolders = [
      ...folder.subFolders,
      ...newSubFolders
    ];

    folder.subFolderName = '';
    folder.isNumberedSubFolder = false;
    folder.subFolderStart = 1;
    folder.subFolderEnd = 4;
    folder.showCreateSubFolder = false;
  }
}