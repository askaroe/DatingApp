import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  title = '';
  list: any;
  closeBtnName = '';
  
  constructor(public bsModalRef: BsModalRef) {}
  
  ngOnInit(): void {
  }

}
