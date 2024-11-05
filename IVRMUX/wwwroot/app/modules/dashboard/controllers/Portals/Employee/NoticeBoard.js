(function () {
    'use strict';
    angular
        .module('app')
        .controller('NoticeBoardController', NoticeBoardController);
    NoticeBoardController.$inject = ['$window', '$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];
    function NoticeBoardController($window, $rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        // $scope.file_temp = [];
        $scope.search = "";
        $scope.intB_DispalyDisableFlg = false;
        $scope.checklink = false;
        //$scope.minDate = Date.now();

        $scope.obj = {};

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.editflag = false;


        //----------load data into page.............
        $scope.loaddata = function () {

            $scope.feegrplist1 = [];
            $scope.feedeflist1 = [];
            $scope.feegrplist = [];
            $scope.feedeflist = [];
            $scope.sectionlist = [];
            $scope.sectionlistarray = [];

            $scope.studentlist = [];
            $scope.studentarray = [];
            $scope.studcount = 0;
            $scope.selectedStudCount = 0;
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;
            $scope.check_student = true;
            $scope.getclass = false;
            $scope.getclasscheck = false;
            $scope.View = true;
            
            var id = 1;
            apiService.getURI("NoticeBoard/Getdetails", id).
                then(function (promise) {
                    $scope.notice_details = promise.notice_details;
                    $scope.departmentdropdown = promise.departmentList;
                    $scope.yearlist = promise.yearlist;
                    $scope.route_list = promise.route_list;                 
                    $scope.obj.intB_EndDate = new Date(promise.academicyear[0].asmaY_To_Date);
                    //added by roopa//
                    $scope.feedeflist = promise.fee_terms;
                    $scope.feegrplist = promise.fee_group;

                    if (promise.classlist !=null && promise.classlist.length > 0) {
                        $scope.classlist = promise.classlist;
                    }
                    if (promise.notice_details != null && promise.notice_details.length > 0) {

                        angular.forEach($scope.notice_details, function (ddd) {
                            if (ddd.intB_FilePath != undefined && ddd.intB_FilePath != '' && ddd.intB_FilePath != null) {
                                var img = ddd.intB_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                ddd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'ppsx') {
                                    ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.intB_FilePath;
                                }
                            }


                        })

                    }

                    console.log($scope.notice_details)
                    $scope.Noticefor = promise.notice_details[0].NoticeType;
                });
        };
        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectpdf = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.pdf'
                    })[0].click();
                });
        };

        var imagedownloadppt = "";
        var docname = "";
        $scope.downloaddirectppt = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownloadppt = data;
            imagedownloadppt = data;

            $http.get(imagedownloadppt, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.ppt || .pptx || .ppsx'
                    })[0].click();
                });
        };
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.jpg'
                    })[0].click();
                });
        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/pptx" || input.files[0].type === "application/ppsx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.slideshow" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }

            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {
                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!
        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');



                });
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        //-------check date
        $scope.display_Date = function () {
            if ($scope.obj.intB_DisplayDate > $scope.obj.intB_EndDate) {
                swal("Display Date Cannot Greater than End date ");
                $scope.obj.intB_DisplayDate = "";
            }
            else if ($scope.obj.intB_StartDate > $scope.obj.intB_EndDate) {
                swal("Start Date Cannot Greater than End date ");
                $scope.obj.intB_EndDate = "";
            }
        };

        //-------------search date
        $scope.filterValue = function (obj) {
            angular.lowercase(obj.intB_Title).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Description).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Attachment).indexOf(angular.lowercase($scope.search)) >= 0 ||
                ($filter('date')(obj.intB_StartDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_EndDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_DisplayDate, 'dd/MM/yyyy').indexOf($scope.search) > 0);
        };
        $scope.filterValue = function (obj) {
            return $filter('date')(obj.OrderDate, 'MM/dd/yyyy') === $filter('date')($scope.searchValue, 'MM/dd/yyyy');
        };

        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequireddesc = function () {
            return !$scope.designationdropdown.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredclass = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.sectionlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredgrp = function () {
            return !$scope.feegrplist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentdropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredemp = function () {
            return !$scope.get_userEmplist.some(function (item) {
                return item.empck;
            });
        };

        $scope.isOptionsRequiredgroup = function () {
            return !$scope.feegrplist.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredterms = function () {
            return !$scope.feedeflist.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredstu = function () {
            return !$scope.studentlist.some(function (item) {
                return item.selected;
            });
        };


        //$scope.isOptionsRequired1 = function () {
        //    return !$scope.studentlist.some(function (item) {
        //        return item.selected;
        //    });
        //};

        //=======selection of checkbox....
        //$scope.togchkbxC = function () {
        //    $scope.usercheckC = $scope.sectionlist.every(function (role) {
        //        return role.selected;
        //    });


        //};
        $scope.togchkbxS = function () {
            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMST_Id: qq.amsT_Id })
                }
                $scope.selectedStudCount = $scope.studentarray.length;
            })
            angular.forEach($scope.studentlist, function (aa) {
                if (aa.selected === false) {
                    $scope.obj.usercheckS = false;
                }
            });
        }
        //fee group
        $scope.togchkbxG = function () {
            $scope.defgrparray = [];
            angular.forEach($scope.feegrplist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ AMST_Id: qq.amsT_Id })
                }
            })
        }

        $scope.togchkbxF = function () {
            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                }
            })
        }
        //
        $scope.all_checkC = function (all, ASMCL_Id) {
            $scope.sectionlistarray = [];
            $scope.obj.usercheckC = all;
            var toggleStatus = $scope.obj.usercheckC;
            angular.forEach($scope.sectionlist, function (role) {
                role.selected = toggleStatus;
            });

            if ($scope.obj.usercheckC == false) {
                $scope.obj.usercheckS = false
            }
            $scope.sectionlistarray = [];
            angular.forEach($scope.sectionlist, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })

                    //$scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })
                }
            });
            $scope.getstudent();
            //added by roopa//
            //$scope.classlistarray = [];
            //angular.forEach($scope.classlist, function (qq) {
            //    if (qq.selected == true) {
            //        $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
            //    }
            //});

            ////
            //var data = {
            //   // "ASMCL_Id": ASMCL_Id,
            //    "classlsttwo": $scope.classlistarray,
            //    "sectionlistarray": $scope.sectionlistarray
            //}
            //apiService.create("NoticeBoard/getstudent", data).then(function (promise) {
            //    $scope.studentlist1 = [];
            //    $scope.studentlist = [];
            //    $scope.studentlist1 = promise.studentlist;
            //    if ($scope.studentlist1.length > 0 || $scope.studentlist1 != null) {
            //        $scope.studentlist = $scope.studentlist1;
            //    }
            //    else {
            //        swal('No Data Found!!!')
            //    }
            //  })

        };
        $scope.all_checkR = function (all, TRMR_Id) {
            $scope.routelistarray = [];
            $scope.obj.usercheckR = all;
            var toggleStatus = $scope.obj.usercheckR;
            angular.forEach($scope.route_list, function (role) {
                role.selected = toggleStatus;
            });

            angular.forEach($scope.route_list, function (qq) {
                if (qq.selected == true) {
                    $scope.routelistarray.push({ TRMR_Id: qq.trmR_Id })
                }
            });
        };
        $scope.getstudent = function () {
            $scope.studentlist = [];
            if ($scope.obj.fee_def == true) {
                $scope.defgrparray = [];
                angular.forEach($scope.feegrplist, function (qq) {
                    if (qq.selected == true) {
                        $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                    }
                });

                $scope.deflistarray = [];
                angular.forEach($scope.feedeflist, function (qq) {
                    if (qq.selected == true) {
                        $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                    }
                });
            }

            if ($scope.obj.fee_def == true) {
                $scope.defgrparray = [];
                angular.forEach($scope.feegrplist, function (qq) {
                    if (qq.selected == true) {
                        $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                    }
                });

                $scope.deflistarray = [];
                angular.forEach($scope.feedeflist, function (qq) {
                    if (qq.selected == true) {
                        $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                    }
                });
            }
            if ($scope.obj.select_route == true) {
                $scope.routearray = [];
                angular.forEach($scope.route_list, function (qq) {
                    if (qq.selected == true) {
                        $scope.routearray.push({ TRMR_Id: qq.trmR_Id })
                    }
                });
            }
            //added by roopa//
            $scope.classlistarray = [];

            if ($scope.classlistarray.length == 0 || $scope.classlistarray == null) {
                angular.forEach($scope.classlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })

                    }
                });
            }

            if ($scope.sectionlist != null && $scope.sectionlist.length > 0) {
                $scope.sectionlistarray = [];
                angular.forEach($scope.sectionlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                       // $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })
                    }
                });
            }

            angular.forEach($scope.sectionlist, function (aa) {
                if (aa.selected === false) {
                    $scope.obj.usercheckC = false;
                }
            });
            //if ($scope.obj.select_student == true) {
            var data = {
                // "ASMCL_Id": ASMCL_Id,
                "classlsttwo": $scope.classlistarray,
                "sectionlistarray": $scope.sectionlistarray,
                "defarray": $scope.deflistarray,
                "defgrparray": $scope.defgrparray,
                "fee_def": $scope.obj.fee_def,
                "routearray": $scope.routearray,
                "routeflag": $scope.obj.select_route
            }
            apiService.create("NoticeBoard/getstudent", data).then(function (promise) {
                $scope.studentlist1 = [];
                $scope.studentlist = [];
                $scope.studentlist1 = promise.studentlist;
                if ($scope.studentlist1.length > 0 || $scope.studentlist1 != null) {
                    $scope.studentlist = $scope.studentlist1;
                    $scope.studcount = promise.studentlist.length;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
            // }
        }
        //==========dept =================
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.arrayuserdept = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });

            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserdept = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserdept
            };
            apiService.create("NoticeBoard/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;

                }
                else {

                    swal("No Record  Found..... !!");
                }
            });
        };
        //===========designation============
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_userEmplist = [];
            $scope.arrayuserdesig = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdesig.push(em);
                }
            });

            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });

            $scope.des_test = true;
            $scope.togchkbxdesg();
        };

        $scope.togchkbxdesg = function (groupType) {
            //if ($scope.des_test == true) {
            //    var data = {
            //        "designationlist": $scope.arrayuserdesig,
            //        "departmentlist": $scope.arrayuserdept
            //    };
            //}
            //else {
                $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                    return itm.selected;
                });
                $scope.arrayuserdesig = [];
                angular.forEach($scope.designationdropdown, function (em) {
                    if (em.selected === true) {
                        $scope.arrayuserdesig.push(em);
                    }
                });

                $scope.arrayuserdept = [];
                angular.forEach($scope.departmentdropdown, function (em) {
                    if (em.selected === true) {
                        $scope.arrayuserdept.push(em);
                    }
                });
                var data = {
                    "designationlist": $scope.arrayuserdesig,
                    "departmentlist": $scope.arrayuserdept,

                };
            //}

            apiService.create("NoticeBoard/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {

                    $scope.des_test = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = false;
                        });
                        $scope.checkall = false;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };
        //---------all checkbox Select...
        $scope.all_check = function (checkall) {
            $scope.userc = checkall;
            var toggleStatus = $scope.userc;
            angular.forEach($scope.get_userEmplist, function (role) {
                role.empck = toggleStatus;
            });

            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }

        $scope.all_checkS = function (all) {
            $scope.obj.usercheckS = all;
            var toggleStatus = $scope.obj.usercheckS;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMST_Id: qq.amsT_Id })
                }
            })

            $scope.selectedStudCount = $scope.studentarray.length;

        };
        // get section
        $scope.getsection = function (ASMCL_Id) {

            //$scope.asmclid = ASMCL_Id;
            //if ($scope.asmclid === "All") {
            //    $scope.classlistarray = [];
            //    $scope.getclass = true;
            //    angular.forEach($scope.classlist, function (aa) {
            //        $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
            //    });
            //}
            //else {
            //    var data = {
            //        "ASMCL_Id": ASMCL_Id
            //    }
            //    apiService.create("NoticeBoard/getsection", data).then(function (promise) {
            //        if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
            //            $scope.sectionlist = promise.sectionlist;
            //            $scope.getclass = false;
            //        }
            //        else {
            //            swal('No data Found!!!');
            //        }
            //    });
            //}


            //added by roopa//
            $scope.classlistarray = [];

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });



            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected === false) {
                    $scope.obj.usercheckCC = false;
                }           
            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }

            var data = {
                "classlsttwo": $scope.classlistarray
            }
            apiService.create("NoticeBoard/getsection", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
            //
        }
        //============employee list======
        $scope.togchkbx = function () {
            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }
        //------------save data.....
        $scope.classlstdata = [];
        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            $scope.classlstdata = [];
            var data = {};
            var displaydate = "";
            //if ($scope.obj.fee_def == true) {
            //    $scope.isOptionsRequiredgroup();
            //    $scope.isOptionsRequiredterms();
            //}

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.intB_DispalyDisableFlg === false) {

                    displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg === true) {
                    displaydate = $scope.obj.intB_DisplayDate === null ? "" : $filter('date')($scope.obj.intB_DisplayDate, "yyyy-MM-dd");
                }

                $scope.filedoc = [];
                $scope.filedoc1 = [];
                $scope.filedoc2 = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.INTBFL_FilePath != null) {
                            $scope.documentListOtherDetails11.push({ INTBFL_FilePath: qq.INTBFL_FilePath, FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }

                if ($scope.checklink == true) {
                    angular.forEach($scope.urldocumentlist, function (ee) {
                        if (ee.INTBFL_FilePath != null) {
                            $scope.filedoc1.push({ INTBFL_FilePath: ee.INTBFL_FilePath, FileName: ee.INTBFL_FilePath });
                        }
                    })
                }
                if ($scope.filedoc1 != null || $scope.filedoc1 > 0) {
                    angular.forEach($scope.filedoc1, function (ww) {
                        $scope.filedoc.push(ww);
                    })
                }
                $scope.studentarraynew = [];
                if ($scope.studentarray.length > 0 || $scope.studentarray != 0) {  
                    $scope.studentarraynew = $scope.studentarray;
                }
                else {
                    angular.forEach($scope.studentlist, function (qq) {
                        $scope.studentarraynew.push({ AMST_Id: qq.amsT_Id });
                    })
                }


                var startdate = $scope.obj.intB_StartDate === null ? "" : $filter('date')($scope.obj.intB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.obj.intB_EndDate === null ? "" : $filter('date')($scope.obj.intB_EndDate, "yyyy-MM-dd");

                //$scope.classlstdata = [];
                //angular.forEach($scope.classlist, function (cls) {
                //    if (cls.selected === true) {
                //        $scope.classlstdata.push(cls);
                //    }
                //});


                $scope.classlistarraynew = [];

                if ($scope.classlistarray != null || $scope.classlistarray > 0) {
                    $scope.classlistarraynew = $scope.classlistarray;
                }
                else {
                    angular.forEach($scope.classlist, function (qq) {
                        $scope.classlistarraynew.push({ ASMCL_Id: qq.asmcL_Id });
                    })
                }

                $scope.sectionlistarraynew = [];

                if ($scope.sectionlistarray != null || $scope.sectionlistarray > 0) {
                    $scope.sectionlistarraynew = $scope.sectionlistarray;
                }
                else {
                    angular.forEach($scope.sectionlist, function (qq) {
                         $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id });
                       // $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id });
                    })
                }

                if ($scope.getclass == true) {
                    data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "INTB_Attachment": $scope.file_detail,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "designationlist": $scope.arrayuserdesig,
                        "departmentlist": $scope.arrayuserdept,
                        "INTB_ToStudentFlg": $scope.check_student,
                        "INTB_ToStaffFlg": $scope.check_staff,
                        "employeearraylist": $scope.employeearraylist,
                        "FilePath_Array": $scope.filedoc,
                        "getclass": $scope.getclass,
                        "classlsttwo": $scope.classlistarraynew,
                        "fee_def": $scope.obj.fee_def,
                        "select_student": $scope.obj.select_student,
                        "defarray": $scope.deflistarray,
                        "defgrparray": $scope.defgrparray
                    };
                }
                else {
                    data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "INTB_Attachment": $scope.file_detail,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg, 
                        "studentarray": $scope.studentarraynew,
                        //"ASMCL_Id": $scope.asmclid,
                        "classlsttwo": $scope.classlistarraynew,
                        "sectionlistarray": $scope.sectionlistarraynew,
                        "designationlist": $scope.arrayuserdesig,
                        "departmentlist": $scope.arrayuserdept,
                        "INTB_ToStudentFlg": $scope.check_student,
                        "INTB_ToStaffFlg": $scope.check_staff,
                        "employeearraylist": $scope.employeearraylist,
                        "FilePath_Array": $scope.filedoc,
                        "fee_def": $scope.obj.fee_def,
                        "select_student": $scope.obj.select_student,
                        "defarray": $scope.deflistarray,
                        "defgrparray": $scope.defgrparray
                    };
                }

                apiService.create("NoticeBoard/savedetail", data).
                    then(function (promise) {
                        $scope.getclasscheck = false;
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.intB_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                        $scope.deviceArray = promise.deviceArray;
                                    }
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.intB_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==============view data=========

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.intB_Id

            };
            apiService.create("NoticeBoard/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist1 = [];
                        angular.forEach(promise.attachementlist, function (qq) {
                            $scope.img = qq.intbfL_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.attachementlist1.push({
                                    INTBFL_FileName: qq.intbfL_FileName,
                                    INTBFL_FilePath: qq.intbfL_FilePath,
                                    INTB_Attachment: qq.intB_Attachment,
                                    INTB_Id: qq.intB_Id
                                })
                            }
                            else {
                                $scope.attachementlist1.push({

                                    INTBFL_FileName: qq.intbfL_FileName,
                                    INTBFL_FilePath: qq.intbfL_FilePath,
                                    INTB_Attachment: 'HyperLink',
                                    INTB_Id: qq.intB_Id

                                })
                            }
                        })

                        $scope.attachementlist = $scope.attachementlist1

                        $('#myModalCoverview').modal('show');
                        $scope.docshowary = true;
                        $scope.docshow = false;
                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };
        //============
        $scope.checkteru = function () {
            if ($scope.check_student == true && $scope.check_staff == false) {
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == true) {
                $scope.check_staff = true;
            }
            else if ($scope.check_student == true && $scope.check_staff == true) {
                $scope.check_staff = true;
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == false) {
                $scope.check_staff = false;
                $scope.check_student = true;
            }
        }
        //----------------edit data.......
        $scope.editnotice = function (id) {
            $scope.checklink = false;
            var data = {
                "INTB_Id": id.intB_Id,
                "classlsttwo": $scope.classlistarray,
                "sectionlistarray": $scope.sectionlistarray,
                "defarray": $scope.deflistarray,
                "defgrparray": $scope.defgrparray,
                "fee_def": $scope.obj.fee_def

            };
            apiService.create("NoticeBoard/editdetails", data).then(function (promise) {
                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {
                    $scope.editflag = true;

                    $scope.intB_Id = promise.editdetails[0].intB_Id;
                    $scope.intB_Title = promise.editdetails[0].intB_Title;
                    $scope.intB_Description = promise.editdetails[0].intB_Description;
                    $scope.file_detail = promise.editdetails[0].intB_Attachment;
                    $scope.obj.intB_StartDate = new Date(promise.editdetails[0].intB_StartDate);
                    $scope.obj.intB_EndDate = new Date(promise.editdetails[0].intB_EndDate);
                    $scope.intB_FilePath = promise.editdetails[0].intB_FilePath;
                    $scope.notice = promise.editdetails[0].intB_FilePath;
                    $scope.ntB_TTSylabusFlg = promise.editdetails[0].ntB_TTSylabusFlg;
                    $scope.intB_DispalyDisableFlg = promise.editdetails[0].intB_DispalyDisableFlg;
                    $scope.check_staff = promise.editdetails[0].intB_ToStaffFlg;
                    $scope.check_student = promise.editdetails[0].intB_ToStudentFlg;
                    $scope.urldocumentlist = [];
                    $scope.documentListOtherDetails = [];
                    if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                        angular.forEach(promise.attachementlist, function (aa) {
                            $scope.img = aa.intbfL_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.documentListOtherDetails.push({
                                    id: 1, INTBFL_FilePath: aa.intbfL_FilePath,
                                    FileName: aa.intbfL_FileName
                                });
                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, INTBFL_FilePath: aa.intbfL_FilePath,
                                    FileName: aa.intbfL_FileName
                                });

                            }
                        });
                    }
                    else {
                        $scope.documentListOtherDetails.push({
                            id: 1, INTBFL_FilePath: '',
                            FileName: ''
                        });
                        //$scope.urldocumentlist.push({
                        //    id: 1, INTBFL_FilePath: '',
                        //    FileName: ''
                        //});
                    }

                    if ($scope.urldocumentlist.length > 0) {
                        $scope.checklink = true;
                    } else {
                        $scope.urldocumentlist = [{ id: 'document' }];
                    }
                    //=============================
                    $scope.classlist = promise.classlist;
                    if (promise.editclasslist != null && promise.editclasslist.length > 0) {
                        $scope.classlistarray = [];


                        $scope.getclass = true;
                        $scope.getclasscheck = true;
                        $scope.ASMCL_Id = "All";
                        //     $scope.classlist = promise.editclasslist;
                        //    $scope.classlist = promise.classlist;
                        if ($scope.classlist.length > 0) {
                            angular.forEach($scope.classlist, function (role) {
                                role.selected = true;


                            });
                        }

                    }

                    //if (promise.sect.length > 0) {
                    //        $scope.sectionlist= promise.sect
                    //        angular.forEach($scope.sectionlist, function (role) {
                    //            role.selected = true;
                    //        });

                    //}
                    //else {

                    //}
                    //if (promise.stu.length > 0) {

                    //    $scope.studentlist = promise.stu;
                    //    angular.forEach($scope.studentlist, function (role) {
                    //        role.selected = true;
                    //    });
                    //}
                }
                $scope.intB_DispalyDisableFlg = false;
                if (promise.editdetails[0].intB_DispalyDisableFlg === true) {
                    $scope.intB_DispalyDisableFlg = true;
                    $scope.obj.intB_DisplayDate = new Date(promise.editdetails[0].intB_DisplayDate);
                }

                $scope.editsection = [];
                $scope.sectionlist = [];
                $scope.editsection = promise.editsection;
                $scope.sectionlist = promise.sectionlist;
                $scope.sectionlistarray = [];
                angular.forEach($scope.sectionlist, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.editsection, function (xx) {

                        if (tt.asmS_Id === xx.asmS_Id) {

                            tt.selected = true;
                            $scope.sectionlistarray.push({ ASMS_Id: xx.asmS_Id })
                        }
                    });
                });

                $scope.studentlist1 = [];
                $scope.studentlist5 = [];
                $scope.studentlistnew = [];
                $scope.studentlist = [];
                //   $scope.studentlistnew = promise.studentlistedit;
                $scope.studentlistnew = promise.studentlist;

                $scope.studentlist1 = promise.editstudent;
                if ($scope.studentlistnew.length > 0) {
                    $scope.obj.select_student
                    //  $scope.studentlist1 = promise.editstudent;
                    angular.forEach($scope.studentlist1, function (qq) {
                        angular.forEach($scope.studentlistnew, function (aa) {
                            if (aa.amsT_Id == qq.amsT_Id) {
                                $scope.studentlist5.push({ amsT_Id: aa.amsT_Id, studentname: aa.studentname, selected: true });
                            }

                        });
                    });

                }
                if ($scope.studentlist5.length > 0 && $scope.studentlist5 != null) {
                    $scope.obj.select_student = true;
                }
                $scope.studentlist = $scope.studentlist5;
                //staff========
                $scope.editstaff = [];
                $scope.get_userEmplist = [];
                $scope.employeearraylist = [];
                $scope.editstaff = promise.editstaff
                $scope.get_userEmplist = promise.get_userEmplist
                angular.forEach($scope.get_userEmplist, function (tt) {
                    tt.empck = false;
                    angular.forEach($scope.editstaff, function (xx) {

                        if (tt.HRME_Id === xx.hrmE_Id) {

                            tt.empck = true;
                            $scope.employeearraylist.push({ HRME_Id: xx.hrmE_Id })
                        }
                    });
                });

                //---------------
                $scope.editdesignation = [];
                $scope.designationdropdown = [];
                $scope.arrayuserdesig = [];
                $scope.editdesignation = promise.editdesignation
                $scope.designationdropdown = promise.designation
                angular.forEach($scope.designationdropdown, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.editdesignation, function (xx) {

                        if (tt.HRMDES_Id === xx.hrmdeS_Id) {

                            tt.selected = true;
                            $scope.arrayuserdesig.push({ HRMDES_Id: xx.hrmdeS_Id })
                        }
                    });
                });

                //---------------
                $scope.editdepartment = [];
                $scope.departmentdropdown = [];
                $scope.arrayuserdept = [];
                $scope.editdepartment = promise.editdepartment
                $scope.departmentdropdown = promise.departmentList
                angular.forEach($scope.departmentdropdown, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.editdepartment, function (xx) {

                        if (tt.HRMDC_ID === xx.hrmdC_ID) {

                            tt.selected = true;
                            $scope.arrayuserdept.push({ HRMDC_ID: xx.hrmdC_ID })
                        }
                    });
                });




            });
        };
        //-----------------------------------
        //$scope.editnotice = function (item) {
        //   
        //    $scope.intb_id = item.intb_id;
        //    $scope.intb_title = item.intb_title;
        //    $scope.intb_description = item.intb_description;
        //    $scope.file_detail = item.intb_attachment;
        //    $scope.intb_filepath = item.intb_filepath;
        //    $scope.notice = item.intb_filepath;
        //    $scope.intb_startdate = new date(item.intb_startdate);
        //    $scope.intb_enddate = new date(item.intb_enddate);
        //    $scope.intb_displaydate = new date(item.intb_displaydate);
        //}
        //-------------for active and deactive
        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.intB_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NoticeBoard/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };
        $scope.clear = function () {
            $state.reload();
        };

        $scope.clearcls = function () {
            // if ($scope.obj.select_student == true) {



            if (($scope.obj.select_student == false && $scope.obj.fee_def == false) || ($scope.obj.select_student == true && $scope.obj.fee_def == true) || ($scope.obj.select_student == false && $scope.obj.select_route == false) || ($scope.obj.select_student == true && $scope.obj.select_route == true)) {
                
                //$scope.usercheckCC = false;
                $scope.obj.usercheckC = false;
                $scope.obj.usercheckCC = false;


                angular.forEach($scope.classlist, function (qq) {
                    if (qq.selected == true) {
                        qq.selected = false;


                    }
                    //else {
                    //    qq.selected = true;
                    //}
                });

                angular.forEach($scope.sectionlist, function (qq) {
                    if (qq.selected == true) {
                        qq.selected = false;

                    }
                    else {

                        qq.selected = true;
                    }
                });



            }
            if ($scope.obj.fee_def == true) {
                $scope.studentlist = [];

            }


            //$scope.classlistarray = [];
            //$scope.sectionlistarray = [];
            //  }
        };

        //============Multiple file upload===========
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //-------------------ADD------------------
        $scope.urldocumentlist = [{ id: 'document' }];
        $scope.addNewurl = function () {
            var newItemNo = $scope.urldocumentlist.length + 1;
            if (newItemNo <= 30) {
                $scope.urldocumentlist.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //======================= file upload
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };

        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/NoticeUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.INTBFL_FilePath = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');



                    });



            }
            else {
                $window.open($scope.imagepreview)
            }
        };

        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }



        $scope.flag = "";
        $scope.flag1 = "";
        $scope.notice_name = "";
        $scope.type_flg = "";
        $scope.type_flg1 = "";
        $scope.View = true;

        $scope.viewrecordstwo = function (user) {

            var data = user;

            //var data = {
            //    "INTB_Id": user.intB_Id

            //};
            apiService.create("NoticeBoard/viewrecords", data).then(function (promise) {
                $scope.viewrecords = [];
                $scope.stuviewlist = [];
                $scope.notviewlist = [];
                $scope.stuNotViewlist = [];
                $scope.count = 0
                $scope.ncount = 0
                $scope.count1 = 0
                $scope.ncount1 = 0
                if (promise !== null) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {
                        $scope.flag = promise.flag;
                        if ($scope.flag == 'Staff') {
                            $scope.type_flg = 'Staff';
                        }
                        $scope.notice_name = promise.notice_Name;
                        $scope.viewrecords = promise.viewlist;
                        $scope.count1 = $scope.viewrecords.length;
                        $scope.studentwise = "";
                        $scope.studentwise = $scope.viewrecords[0].studentwise;
                        $scope.noticetype = $scope.viewrecords[0].noticetype;
                        
                    }
                    if (promise.notViewlist != null && promise.notViewlist.length > 0) {
                        $scope.flag = promise.flag;
                        if ($scope.flag == 'Staff') {
                            $scope.type_flg = 'Staff';
                        }
                        $scope.notice_name = promise.notice_Name;

                        $scope.notviewlist = promise.notViewlist;
                        $scope.ncount1 = $scope.notviewlist.length;
                        $scope.studentwise = "";
                        $scope.studentwise = $scope.viewrecords[0].studentwise;
                        $scope.noticetype = $scope.viewrecords[0].noticetype;

                    }
                    if (promise.stuviewlist != null && promise.stuviewlist.length > 0) {
                        $scope.flag1 = promise.flag1;
                        if ($scope.flag1 == 'Student') {
                            $scope.type_flg1 = 'Student';
                        }
                        $scope.notice_name = promise.notice_Name;

                        $scope.stuviewlist = promise.stuviewlist;
                        $scope.count = $scope.stuviewlist.length;
                        $scope.time = $scope.stuviewlist[0].INTBCSTDV_CreatedDate;
                    }
                    if (promise.stuNotViewlist != null && promise.stuNotViewlist.length > 0) {
                        $scope.flag1 = promise.flag1;
                        if ($scope.flag1 == 'Student') {
                            $scope.type_flg1 = 'Student';
                        }
                        $scope.notice_name = promise.notice_Name;
                        $scope.stuNotViewlist = promise.stuNotViewlist;
                        $scope.ncount = $scope.stuNotViewlist.length;
                    }
                }

            });
        };
        //$scope.flag = "";
        //$scope.notice_name = "";
        //$scope.viewrecordstwo = function (user) {

        //    var data = user;

        //    //var data = {
        //    //    "INTB_Id": user.intB_Id

        //    //};
        //    apiService.create("NoticeBoard/viewrecords", data).then(function (promise) {
        //        $scope.viewrecords = [];
        //        if (promise !== null) {
        //            if (promise.viewlist != null && promise.viewlist.length > 0) {
        //                $scope.flag = promise.flag;

        //                $scope.notice_name = promise.notice_Name;

        //                $scope.viewrecords = promise.viewlist; 
        //                $scope.studentwise = "";    
        //                $scope.studentwise = $scope.viewrecords[0].studentwise;
                  
        //            }

        //        }

        //    });
        //};

        //added by roopa//
        $scope.al_checkclass = function (all, ASMCL_Id) {



           $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });

            if ($scope.obj.usercheckCC == false) {
                $scope.obj.usercheckC = false
                $scope.obj.usercheckS = false
            }

            $scope.classlistarray = [];
            angular.forEach($scope.newuser2, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id})
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getsection();
                $scope.classflag = true;
            }
            else {
                $scope.sectionlist = [];
                $scope.studentlist = [];
            }

        }

        $scope.all_checkG = function (all, FMG_Id) {

            $scope.defgrparray = [];
            $scope.usercheckG = all;
            var toggleStatus = $scope.usercheckG;
            angular.forEach($scope.feegrplist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.defgrparray = [];
            angular.forEach($scope.feegrplist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                }
            });


        }

        $scope.all_checkF = function (all, FMT_Id) {

            $scope.deflistarray = [];
            $scope.usercheckF = all;
            var toggleStatus = $scope.usercheckF;
            angular.forEach($scope.feedeflist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMT_Id: qq.fmT_Id })
                }
            });

        }

        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("NoticeBoard/get_noticelist", data).then(function (promise) {
                if (promise.classlist.length > 0) {

                    $scope.classwork = promise.classlist;

                }
                else {
                    swal('Data Not Available');
                    $scope.asmcL_Id = "";
                }
            });
        };
        //====================end==================
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };



    });



})();
