﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_AC_653_IQACController', NAAC_AC_653_IQACController)

    NAAC_AC_653_IQACController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce','myFactorynaac']
    function NAAC_AC_653_IQACController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, myFactorynaac) {


        $scope.ProgramDetails = {};
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        //======================page load
        $scope.loaddata = function () {
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("NAAC_AC_653_IQAC/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.allacademicyear = promise.allacademicyear;
                $scope.alldata1 = promise.alldata1;
                $scope.gridOptions.data = promise.alldata1;
            })
        };

        //====================grid data
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year', width: 100 },
                { name: 'name', displayName: 'Quality Name', width: 150 },
                { name: 'duration', displayName: 'Duration', width: 100 },
                { name: 'fdate', displayName: 'Date' },
                { name: 'totalCount', displayName: 'No Of Participants', width: 100 },
               
                {
                    name: 'teacher', displayName: 'Files', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewteacherguides(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.flag1 === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactiveStudent(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.flag1 === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactiveStudent(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.viewteacherguides = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "NCAC653IQAC_Id": obj.ncaC653IQAC_Id,
                "MI_Id": obj.mI_Id
            };

            apiService.create("NAAC_AC_653_IQAC/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.savedresult;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.savedresult;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.filepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.showGuardianPhoto = function (data) {
            imagedownload = data.lpmtR_Resources;
            docname = data.fileName;
            $('#preview').attr('src', data.lpmtR_Resources);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
            console.log($scope.view_videos);
        };

        //$scope.downloadview = function (pdfview) {
        //    $scope.pdfurl = pdfview;
        //    $scope.showpdf = true;
        //    $('#showpdf').modal('show');
        //};        
        $scope.downloadview = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };
        //====================for delete File
        $scope.deleteuploadfile = function (obj) {
            var data = obj;
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NAAC_AC_653_IQAC/deleteuploadfile", data).then(function (promise) {
                            if (promise.already_cnt === true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record Deleted successfully");
                                }
                                else {
                                    swal("Record Deletion Failed");
                                }
                            }
                            $('#popup11').modal('hide');
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
            debugger;
            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.from_date).toDateString();
              //  $scope.to_date = new Date($scope.to_date).toDateString();

                var data = {
                    "NCAC653IQAC_Id": $scope.NCAC653IQAC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Name": $scope.projectname,
                    "duration": $scope.count,
                    "fromdate": $scope.from_date,
                    "pgTempDTO": $scope.ProgramDetails,
                    "NCAC653IQAC_Venue": $scope.NCAC653IQAC_Venue,
                    "NCAC653IQAC_NoOfTeacher": $scope.NCAC653IQAC_NoOfTeacher,
                    "TotalCount": $scope.Participant,
                    "NCAC653IQAC_RegIQACFlg": $scope.NCAC653IQAC_RegIQACFlg,
                    "NCAC653IQAC_FeedbackClgImprts": $scope.NCAC653IQAC_FeedbackClgImprts,
                    "NCAC653IQAC_PrepOfDocAccBodiesFlg": $scope.NCAC653IQAC_PrepOfDocAccBodiesFlg,
                    "MI_Id": $scope.mI_Id
                }

                apiService.create("NAAC_AC_653_IQAC/save", data).then(function (promise) {
                    debugger;
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully")
                        $state.reload();
                    }
                    else if (promise.msg == 'failed') {
                        swal("Data is not Saved");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.msg == 'upfailed') {
                        swal("Data is not updated");
                    }
                    else if (promise.returnval == true) {
                        swal("Data is already Existing");
                    }
                    else {
                        swal("Something is Wrong......");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }


        }

        $scope.deactiveStudent = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.flag1 == true) {
                dystring = "Deactivate";
            }
            else if (usersem.flag1 == false) {
                dystring = "Activate"
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NAAC_AC_653_IQAC/deactiveStudent", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        //-----------upload file/photo.............
        $scope.ProgramDetails = [{ id: 'photo1' }];
        
        $scope.addNewsiblingguard = function () {
        
            var newItemNo = $scope.ProgramDetails.length + 1;

            if (newItemNo <= 10) {
                $scope.ProgramDetails.push({ 'id': 'photo' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.ProgramDetails.length - 1;
            $scope.ProgramDetails.splice(index, 1);

            if ($scope.ProgramDetails.length === 0) {
                //data
            }
        };
        //----------End..........!

        $scope.UploadPhoto = function (input, document) {
          
            $scope.UploadPhoto1 = input.files;
            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                    input.files[0].type == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploaddianPhotovideo(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }


        };

        function UploaddianPhotovideo(data) {
          
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadPhoto1.length; i++) {
                formData.append("File", $scope.UploadPhoto1[i]);
                // $scope.file_detail = $scope.UploadPhoto1[0].name;
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.lpmtR_Resources = d;
                    data.file_name = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.toString().split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
                    }
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //================================edit data
        $scope.institute_flag = false;
        $scope.EditData = function (user) {
            var data = {
                "NCAC653IQAC_Id": user.ncaC653IQAC_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NAAC_AC_653_IQAC/EditData", data).then(function (promise) {
                debugger;
                if (promise.editlist.length > 0) {
                    $scope.institute_flag = true;
                    $scope.NCAC653IQAC_Id = promise.editlist[0].ncaC653IQAC_Id;
                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                    $scope.projectname = promise.editlist[0].name;
                    $scope.count = promise.editlist[0].duration;
                    $scope.NCAC653IQAC_Venue = promise.editlist[0].ncaC653IQAC_Venue;
                    $scope.NCAC653IQAC_NoOfTeacher = promise.editlist[0].ncaC653IQAC_NoOfTeacher;
                    $scope.Participant = promise.editlist[0].totalCount;
                    $scope.from_date = new Date(promise.editlist[0].fromdate);
                    $scope.mI_Id = promise.editlist[0].mI_Id;

                    if (promise.editlist[0].ncaC653IQAC_RegIQACFlg == true) {
                        $scope.NCAC653IQAC_RegIQACFlg = 1;
                    }
                    if (promise.editlist[0].ncaC653IQAC_FeedbackClgImprts==true) {
                        $scope.NCAC653IQAC_FeedbackClgImprts = 1;
                    }
                    if (promise.editlist[0].ncaC653IQAC_PrepOfDocAccBodiesFlg==true) {
                        $scope.NCAC653IQAC_PrepOfDocAccBodiesFlg = 1;
                    }                  
                   

                    $scope.ProgramDetails = promise.editfiles;
                    if ($scope.ProgramDetails !== null && $scope.ProgramDetails.length > 0) {

                        angular.forEach($scope.ProgramDetails, function (dd) {
                            var img = dd.filepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            dd.lpmtR_Resources = dd.filepath;
                            dd.desc = dd.description;
                            dd.file_name = dd.fileName;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                            }
                        });
                    } else {
                        $scope.ProgramDetails = [{ id: 'photo1' }];
                    }
                }
            })

        }
         //=================Change of Institution.
        $scope.change_institution = function () {
            $scope.ASMAY_Id = "";
            $scope.projectname = "";
            $scope.count = "";
            $scope.Participant = "";
            $scope.NCAC653IQAC_NoOfTeacher = "";
            $scope.NCAC653IQAC_Venue = "";
            $scope.from_date = "";
            $scope.ProgramDetails = [];
            $scope.submitted = false;
            $scope.ProgramDetails = [{ id: 'photo1' }];
        }




        // view row wise comments
        $scope.getorganizationdata = function (obj) {
            debugger;
            apiService.create("NAAC_AC_653_IQAC/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view file wise comments
        $scope.getorganizationdata1 = function (obj) {
            debugger;
            apiService.create("NAAC_AC_653_IQAC/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            debugger;
            $scope.ccc = obje.ncaC653IQAC_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            debugger;
            $scope.cc = obje.ncaC653IQACF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** save row wise comments ***************//
        $scope.savedatawisecomments = function (obj) {
            debugger;
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("NAAC_AC_653_IQAC/savemedicaldatawisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $('#mymodalviewuploaddocument').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };


        // save file wise comments 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            debugger;
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("NAAC_AC_653_IQAC/savefilewisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments1').modal('hide');
                    $('#mymodalviewuploaddocument1').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };








    }
})();