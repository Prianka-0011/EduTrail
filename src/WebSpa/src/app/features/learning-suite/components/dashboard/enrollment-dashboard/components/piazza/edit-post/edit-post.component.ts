import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PostService } from '../../../../services/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomCategory } from '../../../../../../../../shared/interface/customCategory';
import { IDropdownItem } from '../../../../../../../../shared/interface/iDropdownItem';
import { EditorType, IFolderTreeDto, IPostDetail } from '../../../../interfaces/IPost';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { QuillModule } from 'ngx-quill';
import { MatRadioModule } from '@angular/material/radio';

@Component({
  selector: 'app-edit-post',
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSlideToggleModule,
    QuillModule,
    MatRadioModule
  ],
  templateUrl: './edit-post.component.html',
  styleUrl: './edit-post.component.scss'
})
export class EditPostComponent implements OnInit {
  CustomCategory = CustomCategory;
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  enrollements: IDropdownItem[] = [];
  types: IDropdownItem[] = [];
  folders: IFolderTreeDto[] = [];


  postDetail: IPostDetail = {
    id: '',

    summary: '',
    details: '',

    postTypeId: '',
    postTypeName: '',

    editorType: EditorType.RichText,

    isAnnouncement: false,
    sendEmailImmediately: false,
    isScheduled: false,

    poll: {
      question: '',
      options: [
        { optionText: '', voteCount: 0  },
        { optionText: '', voteCount: 0  }
      ]
    }
  };


  postTo = 'class';

  showPreview = false;


  constructor(
    private postService: PostService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toast: ToastrService
  ) { }


  ngOnInit(): void {

    const courseOfferingId =
      this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId')
      ?? this.EMPTY_ID;


    const postId =
      this.activeRoute.snapshot.queryParamMap.get('id')
      ?? this.EMPTY_ID;


    this.postService.getPostById(postId, courseOfferingId)
      .subscribe({

        next: res => {

          this.postDetail.enrollmentIds = this.postDetail.enrollmentIds ?? [];
          this.postDetail.folderIds = this.postDetail.folderIds ?? [];

          this.postDetail = res.detailsDto ?? this.postDetail;
          this.postTo = res.detailsDto?.isIndividual == true ? "individual" : "class"
          this.types = res.types ?? [];

          this.folders = res.folders ?? [];

          this.enrollements = res.enrollements ?? [];


          if (!this.postDetail.poll) {

            this.postDetail.poll = {
              options: [
                { optionText: '', voteCount: 0 },
                { optionText: '', voteCount: 0 }
              ]
            };

          }

        },

        error: err => {
          console.error(err);
        }

      });

  }



  onPostTypeChange(event: any) {

    if (this.postDetail.postTypeId === CustomCategory.PostTypes.Poll) {

      this.postDetail.poll = {
        question: '',
        options: [
          { optionText: '', voteCount: 0 },
          { optionText: '', voteCount: 0 }
        ]
      };

    } else {

      // this.postDetail.poll = undefined;

    }
  }

  addPollOption(): void {
    if (!this.postDetail.poll) {

      this.postDetail.poll = {
        options: []
      };

    }

    this.postDetail.poll.options.push({
      optionText: '',
      voteCount: 0
    });

  }

  removePollOption(index: number): void {

    if (!this.postDetail.poll) {
      return;
    }

    if (this.postDetail.poll.options.length > 2) {

      this.postDetail.poll.options.splice(index, 1);

    }

  }

  onPostToChange(): void {
    if (this.postTo === 'class') {
      this.postDetail.enrollmentIds = [];
    }
  }

  close(): void {
    this.postDetail = {

      id: '',

      summary: '',
      details: '',

      postTypeId: CustomCategory.PostTypes.Note,

      editorType: EditorType.RichText,

      poll: undefined

    };

    this.postTo = 'class';

    // this.selectedUsers = [];

    // this.selectedFolderIds = [];

    this.showPreview = false;

  }

  getSelectedUserNames(): string {
    return this.enrollements
      .filter(user => this.postDetail.enrollmentIds?.includes(user.id))
      .map(user => user.name)
      .join(', ');
  }


  getSelectedFolderNames(): string {
    const allFolders = this.flattenFolders(this.folders);

    return allFolders
      .filter(folder => this.postDetail.folderIds?.includes(folder.id))
      .map(folder => folder.name)
      .join(', ');
  }


  private flattenFolders(folders: IFolderTreeDto[]): IFolderTreeDto[] {
    return folders.reduce((result, folder) => {
      result.push(folder);

      if (folder.childFolders?.length) {
        result.push(...this.flattenFolders(folder.childFolders));
      }

      return result;
    }, [] as IFolderTreeDto[]);
  }

  submitPost(): void {

    const payload = {
      ...this.postDetail,

      isIndividual: this.postTo === 'individual',

      enrollmentIds: this.postTo === 'individual'
        ? this.postDetail.enrollmentIds
        : [],

      folderIds: this.postDetail.folderIds
    };

    this.postService.updatePost(payload)
      .subscribe({
        next: () => {
          this.toast.success('Post created successfully');
          this.cancel.emit();
        },

        error: err => {
          console.error(err);
          this.toast.error('Failed to create post');
        }

      });
  }

}