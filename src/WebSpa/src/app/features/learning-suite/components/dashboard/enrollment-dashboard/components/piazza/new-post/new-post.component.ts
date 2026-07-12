import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { QuillModule } from 'ngx-quill';

import { PostService } from '../../../../services/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { EditorType, IFolderTreeDto, IPostDetail } from '../../../../interfaces/IPost';
import { IDropdownItem } from '../../../../../../../../shared/interface/iDropdownItem';
import { CustomCategory } from '../../../../../../../../shared/interface/customCategory';


@Component({
  selector: 'app-new-post',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSlideToggleModule,
    QuillModule
  ],
  templateUrl: './new-post.component.html',
  styleUrl: './new-post.component.scss'
})
export class NewPostComponent implements OnInit {
  CustomCategory = CustomCategory;
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';

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
        { optionText: '' },
        { optionText: '' }
      ]
    }
  };


  postTo = 'class';

  showPreview = false;

  editorType = 'rich';


  selectedFolderIds: string[] = [];

  selectedUsers: string[] = [];


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
      this.activeRoute.parent?.snapshot.paramMap.get('postId')
      ?? this.EMPTY_ID;


    this.postService.getPostById(postId, courseOfferingId)
      .subscribe({

        next: res => {

          this.postDetail = res.detailsDto ?? this.postDetail;

          this.types = res.types ?? [];

          this.folders = res.folders ?? [];

          this.enrollements = res.enrollements ?? [];


          if (!this.postDetail.poll) {

            this.postDetail.poll = {
              options: [
                { optionText: '' },
                { optionText: '' }
              ]
            };

          }

        },

        error: err => {
          console.error(err);
        }

      });

  }



  onPostTypeChange(): void {

    if (this.postDetail.postTypeId === CustomCategory.PostTypes.Poll) {

      this.postDetail.poll = {
        question: '',
        options: [
          { optionText: '' },
          { optionText: '' }
        ]
      };

    } else {

      this.postDetail.poll = undefined;

    }
  }

  addPollOption(): void {

    if (!this.postDetail.poll) {

      this.postDetail.poll = {
        options: []
      };

    }


    this.postDetail.poll.options.push({

      optionText: ''

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

      this.selectedUsers = [];

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

    this.selectedUsers = [];

    this.selectedFolderIds = [];

    this.showPreview = false;

  }



  submitPost(): void {


    const payload = {

      ...this.postDetail,

      folderIds: this.selectedFolderIds,

      enrollmentIds:
        this.postTo === 'individual'
          ? this.selectedUsers
          : []

    };


    console.log(payload, "Hello Payload");


    this.postService.createPost(payload)
      .subscribe({

        next: () => {

          this.toast.success('Post created successfully');

          this.router.navigate(['../'], {
            relativeTo: this.activeRoute
          });

        },

        error: err => {

          console.error(err);

          this.toast.error('Failed to create post');

        }

      });


  }

}