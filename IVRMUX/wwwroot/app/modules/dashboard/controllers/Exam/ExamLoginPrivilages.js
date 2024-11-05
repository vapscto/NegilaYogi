(function () {
    'use strict';
    angular.module('app').controller('ExamLoginPrivilagesController', ExamLoginPrivilagesController)
    ExamLoginPrivilagesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamLoginPrivilagesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.details = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.disclass = true;
        $scope.dissection = true;
        $scope.dis = false;
        $scope.clldis = true;
        $scope.obj123 = {};
        $scope.obj1234 = {};
        $scope.act = 'add';
        $scope.BindData = function () {
            apiService.getDATA("ExamLoginPrivilages/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.subjectlt = promise.subjlist;
                $scope.subsubjlt = promise.subsubject;
                $scope.emplist = promise.emplist;
                $scope.userlist = promise.userlist;
                $scope.tempemplt = promise.emplist;
                $scope.tempusrlt = promise.userlist;
                $scope.gridOptions.data = promise.pllist;
                $scope.selectemplt = [];
                angular.forEach($scope.emplist, function (opt) {
                    angular.forEach(promise.clastechlt, function (copt) {
                        if (opt.hrmE_Id == copt) {
                            $scope.selectemplt.push(opt);
                        }
                    });
                });

                $scope.selectusrlt = [];
                angular.forEach($scope.userlist, function (opt) {
                    angular.forEach(promise.clastechlt, function (copt) {
                        if (opt.emp_Code == copt) {
                            $scope.selectusrlt.push(opt);
                        }
                    });
                });
                $scope.temp_clsteas = promise.clastechlt;
                $scope.emplt = $scope.selectemplt;
                $scope.usrlt = $scope.selectusrlt;
            });
        };

        //Class teacher
        $scope.classteacher = function () {
            $scope.clldis = true;
            $scope.emplt = $scope.selectemplt;
            $scope.usrlt = $scope.selectusrlt;
            $scope.asmaY_Id = "";
            $scope.ivrmulF_Id = "";
            $scope.hrmE_Id = "";
            angular.forEach($scope.clslist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.seclist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.subjectlt, function (obj) {
                obj.checkedsub = false;
            });
            angular.forEach($scope.subsubjlt, function (obj) {
                obj.checked = false;
            });

            $scope.selectemplt = [];
            angular.forEach($scope.emplist, function (opt) {
                angular.forEach($scope.temp_clsteas, function (copt) {
                    if (opt.hrmE_Id == copt) {
                        $scope.selectemplt.push(opt);
                    }
                });
            });

            $scope.selectusrlt = [];
            angular.forEach($scope.userlist, function (opt) {
                angular.forEach($scope.temp_clsteas, function (copt) {
                    if (opt.emp_Code == copt) {
                        $scope.selectusrlt.push(opt);

                    }
                });
            });

            $scope.emplt = $scope.selectemplt;
            $scope.usrlt = $scope.selectusrlt;
        };

        //subject teacher 
        $scope.subjectteacher = function () {
            $scope.clldis = false;
            $scope.emplt = $scope.tempemplt;
            $scope.usrlt = $scope.tempusrlt;
            $scope.asmaY_Id = "";
            $scope.ivrmulF_Id = "";
            $scope.hrmE_Id = "";
            angular.forEach($scope.clslist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.seclist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.subjectlt, function (obj) {
                obj.checkedsub = false;
            });
            angular.forEach($scope.subsubjlt, function (obj) {
                obj.checked = false;
            });
        };

        //other
        $scope.others = function () {
            $scope.clldis = false;
            $scope.emplt = $scope.tempemplt;
            $scope.usrlt = $scope.tempusrlt;
            $scope.asmaY_Id = "";
            $scope.ivrmulF_Id = "";
            $scope.hrmE_Id = "";

            angular.forEach($scope.clslist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.seclist, function (obj) {
                obj.checked = false;
            });
            angular.forEach($scope.subjectlt, function (obj) {
                obj.checkedsub = false;
            });
            angular.forEach($scope.subsubjlt, function (obj) {
                obj.checked = false;
            });
        };

        $scope.selectemp = function (id, rid) {
            angular.forEach($scope.usrlt, function (opt) {
                if (opt.ivrmulF_Id == id.ivrmulF_Id.ivrmulF_Id) {
                    $scope.usr_id = opt.emp_Code;
                    $scope.ivrmulF_Id = id.ivrmulF_Id.ivrmulF_Id;
                }
            });
            angular.forEach($scope.emplt, function (opt) {
                if (opt.hrmE_Id == $scope.usr_id) {
                    $scope.hrmE_Id = $scope.usr_id;
                    $scope.obj1234.hrmE_Id = opt;
                    opt.Selected = true;

                }
            });

            if (rid == 'ct') {
                var data = {
                    "HRME_Id": $scope.hrmE_Id,
                    "ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("ExamLoginPrivilages/getclstechdetails", data).then(function (promise) {
                    angular.forEach($scope.clslist, function (opt) {
                        opt.checked = false;
                    });
                    angular.forEach($scope.seclist, function (opt) {
                        opt.checked = false;
                    });
                    angular.forEach($scope.subjectlt, function (opt) {
                        opt.checkedsub = false;
                    });
                    if (promise.clastechlt.length > 0) {
                        if ($scope.selectedclass.length > 0) {
                            angular.forEach($scope.selectedclass, function (s1) {
                                if (s1.yearid == $scope.asmaY_Id && s1.user_id == $scope.ivrmulF_Id) {
                                    angular.forEach($scope.subjectlt, function (stu2) {
                                        angular.forEach(s1.sub, function (s2) {
                                            if (stu2.ismS_Id == s2.ismS_Id) {
                                                stu2.checkedsub = true;
                                            }
                                        });
                                    });
                                }
                            });
                        }

                        for (var i = 0; i < promise.clastechlt.length; i++) {

                            angular.forEach($scope.clslist, function (opt) {
                                if (opt.asmcL_Id == promise.clastechlt[i].asmcL_Id) {
                                    opt.checked = true;
                                }
                            });
                            angular.forEach($scope.seclist, function (opt) {
                                if (opt.asmS_Id == promise.clastechlt[i].asmS_Id) {
                                    opt.checked = true;
                                }
                            });
                        }

                    } else {
                        swal('No Record Found');
                    }
                });
            }

            if (rid == 'st' || $scope.all == 'others') {
                angular.forEach($scope.clslist, function (opt) {
                    opt.checked = false;
                });
                angular.forEach($scope.seclist, function (opt) {
                    opt.checked = false;
                });
                angular.forEach($scope.subjectlt, function (opt) {
                    opt.checkedsub = false;
                });
            }
        };

        $scope.selectemp1 = function (id, rid) {
            angular.forEach($scope.emplt, function (opt) {
                if (opt.hrmE_Id == id.hrmE_Id.hrmE_Id) {
                    $scope.usr_id = opt.hrmE_Id;
                }
            });
            angular.forEach($scope.usrlt, function (opt) {
                if (opt.emp_Code == $scope.usr_id) {
                    $scope.ivrmulF_Id = opt.ivrmulF_Id;
                    opt.Selected = true;
                    $scope.obj123.ivrmulF_Id = opt;
                }
            });
            if (rid == 'ct') {
                var data = {
                    "HRME_Id": id.hrmE_Id.hrmE_Id,
                    "ASMAY_Id": $scope.asmaY_Id
                };

                apiService.create("ExamLoginPrivilages/getclstechdetails", data).then(function (promise) {
                    angular.forEach($scope.clslist, function (opt) {
                        opt.checked = false;
                    });
                    angular.forEach($scope.seclist, function (opt) {
                        opt.checked = false;
                    });
                    angular.forEach($scope.subjectlt, function (opt) {
                        opt.checkedsub = false;
                    });

                    if (promise.clastechlt.length > 0) {
                        if ($scope.selectedclass.length > 0) {
                            angular.forEach($scope.selectedclass, function (s1) {
                                if (s1.yearid == $scope.asmaY_Id && s1.user_id == $scope.ivrmulF_Id) {
                                    angular.forEach($scope.subjectlt, function (stu2) {
                                        angular.forEach(s1.sub, function (s2) {
                                            if (stu2.ismS_Id == s2.ismS_Id) {
                                                stu2.checkedsub = true;
                                            }
                                        });
                                    });
                                }
                            });
                        }
                        for (var i = 0; i < promise.clastechlt.length; i++) {
                            angular.forEach($scope.clslist, function (opt) {
                                if (opt.asmcL_Id == promise.clastechlt[i].asmcL_Id) {
                                    opt.checked = true;
                                }
                            });
                            angular.forEach($scope.seclist, function (opt) {
                                if (opt.asmS_Id == promise.clastechlt[i].asmS_Id) {
                                    opt.checked = true;
                                }
                            });
                        }
                    } else {
                        swal('No Record Found');
                    }
                });
            }

            if (rid == 'st' || $scope.all == 'others') {
                angular.forEach($scope.clslist, function (opt) {
                    opt.checked = false;
                });
                angular.forEach($scope.seclist, function (opt) {
                    opt.checked = false;
                });
                angular.forEach($scope.subjectlt, function (opt) {
                    opt.checkedsub = false;
                });

                if ($scope.selectedclass.length > 0) {
                    angular.forEach($scope.selectedclass, function (s1) {
                        if (s1.yearid == $scope.asmaY_Id && s1.user_id == $scope.ivrmulF_Id) {
                            angular.forEach($scope.clslist, function (stu2) {
                                if (stu2.asmcL_Id == s1.clas.asmcL_Id) {
                                    stu2.checked = true;
                                }
                            });

                            angular.forEach($scope.seclist, function (stu2) {
                                angular.forEach(s1.secs, function (s2) {
                                    if (stu2.asmS_Id == s2.asmS_Id) {
                                        stu2.checked = true;
                                    }
                                });
                            });

                            angular.forEach($scope.subjectlt, function (stu2) {
                                angular.forEach(s1.sub, function (s2) {
                                    if (stu2.ismS_Id == s2.ismS_Id) {
                                        stu2.checkedsub = true;
                                    }
                                });
                            });
                        }
                    });
                }
            }
        };


        $scope.selectedclass = [];
        $scope.submitted = false;
        $scope.Mapsubject = function () {
            $scope.searchchkbx = '';
            if ($scope.myForm.$valid) {
                $scope.subval = [];
                angular.forEach($scope.subjectlt, function (mm) {
                    if (mm.checkedsub == true) {
                        $scope.subval.push(mm);
                    }
                });

                if ($scope.subval.length > 0) {
                    if ($scope.all == 'ct') {
                        angular.forEach($scope.yearlt, function (stu) {
                            if ($scope.asmaY_Id == stu.asmaY_Id) {
                                $scope.yearname = stu.asmaY_Year;
                                $scope.yerId = stu.asmaY_Id;
                            }
                        });

                        angular.forEach($scope.emplt, function (stu) {
                            if ($scope.hrmE_Id == stu.hrmE_Id) {
                                $scope.empname = stu.hrmE_EmployeeFirstName;
                            }
                        });

                        angular.forEach($scope.usrlt, function (stu) {
                            if ($scope.ivrmulF_Id == stu.ivrmulF_Id) {
                                $scope.ivrmulF_Id = stu.ivrmulF_Id;
                            }
                        });

                        $scope.stusec = {};
                        if ($scope.selectedclass.length > 0) {
                            var cnn = 0;
                            angular.forEach($scope.selectedclass, function (si) {
                                if (si.yearid == $scope.yerId && si.user_id == $scope.ivrmulF_Id) {
                                    cnn += 1;
                                    $scope.selectedsection = [];
                                    angular.forEach($scope.seclist, function (stu1) {
                                        if (stu1.checked == true) {
                                            $scope.selectedsection.push(stu1);
                                            $scope.stusec = stu1;
                                        }
                                    });

                                    angular.forEach($scope.clslist, function (stu) {
                                        if (stu.checked == true) {
                                            var a = $scope.selectedclass;
                                            if (stu.asmcL_Id == si.clas.asmcL_Id) {
                                                si.sub = [];
                                                angular.forEach($scope.subjectlt, function (stu2) {
                                                    if (stu2.checkedsub == true) {
                                                        var alou_cnt = 0;
                                                        angular.forEach(si.sub, function (ss) {
                                                            if (ss.ismS_Id == stu2.ismS_Id) {
                                                                alou_cnt += 1;
                                                            }
                                                        });
                                                        if (alou_cnt == 0) {
                                                            si.sub.push(stu2);
                                                        }
                                                    }
                                                });

                                                //subsub
                                                si.ssub = [];
                                                angular.forEach($scope.subsubjlt, function (stu2) {
                                                    if (stu2.checked == true) {
                                                        var ctt = 0;
                                                        angular.forEach(si.ssub, function (ss) {
                                                            if (ss.emsS_Id == stu2.emsS_Id) {
                                                                ctt += 1;
                                                            }
                                                        });
                                                        if (ctt == 0) {
                                                            si.ssub.push(stu2);
                                                        }
                                                    }
                                                });
                                                //secs
                                                angular.forEach($scope.seclist, function (stu2) {
                                                    if (stu2.checked == true) {
                                                        var ssc = 0;
                                                        angular.forEach(si.secs, function (ss) {
                                                            if (ss.asmS_Id == stu2.asmS_Id) {
                                                                ssc += 1;
                                                            }
                                                        });
                                                        if (ssc == 0) {
                                                            si.secs.push(stu2);
                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    });
                                }
                            });

                            if (cnn == 0) {
                                angular.forEach($scope.yearlt, function (stu) {
                                    if ($scope.asmaY_Id == stu.asmaY_Id) {
                                        $scope.yearname = stu.asmaY_Year;
                                        $scope.yerId = stu.asmaY_Id;
                                    }
                                });
                                angular.forEach($scope.emplt, function (stu) {
                                    if ($scope.hrmE_Id == stu.hrmE_Id) {
                                        $scope.empname = stu.hrmE_EmployeeFirstName;
                                    }
                                });
                                angular.forEach($scope.usrlt, function (stu) {
                                    if ($scope.ivrmulF_Id == stu.ivrmulF_Id) {
                                        $scope.ivrmulF_Id = stu.ivrmulF_Id;
                                    }
                                });

                                angular.forEach($scope.clslist, function (stu) {
                                    if (stu.checked == true) {
                                        $scope.selectedsection = [];
                                        angular.forEach($scope.seclist, function (stu1) {
                                            if (stu1.checked == true) {
                                                $scope.selectedsection.push(stu1);
                                                $scope.stusec = stu1;
                                            }
                                        });
                                        $scope.selectedsubject = [];
                                        angular.forEach($scope.subjectlt, function (stu2) {
                                            if (stu2.checkedsub == true) {
                                                $scope.selectedsubject.push(stu2);
                                            }
                                        });
                                        $scope.selectedssubject = [];
                                        angular.forEach($scope.subsubjlt, function (stu3) {
                                            if (stu3.checked == true) {
                                                $scope.selectedssubject.push(stu3);
                                            }
                                        });
                                        $scope.selectedclass.push({
                                            year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                            emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: $scope.stusec,
                                            sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                        });
                                        $scope.details = false;
                                    }
                                });
                            }
                            if (cnn > 0) {
                                //data
                            }
                            $scope.clearpush_data();
                        }

                        else {

                            if ($scope.all == 'ct') {
                                angular.forEach($scope.yearlt, function (stu) {
                                    if ($scope.asmaY_Id == stu.asmaY_Id) {
                                        $scope.yearname = stu.asmaY_Year;
                                        $scope.yerId = stu.asmaY_Id;
                                    }
                                })
                                angular.forEach($scope.emplt, function (stu) {
                                    if ($scope.hrmE_Id == stu.hrmE_Id) {
                                        $scope.empname = stu.hrmE_EmployeeFirstName;
                                    }
                                });
                                angular.forEach($scope.usrlt, function (stu) {
                                    if ($scope.ivrmulF_Id == stu.ivrmulF_Id) {
                                        $scope.ivrmulF_Id = stu.ivrmulF_Id;
                                    }
                                });

                                angular.forEach($scope.clslist, function (stu) {
                                    if (stu.checked == true) {
                                        $scope.selectedsection = [];
                                        angular.forEach($scope.seclist, function (stu1) {
                                            if (stu1.checked == true) {
                                                $scope.selectedsection.push(stu1);
                                                $scope.stusec = stu1;
                                            }
                                        });
                                        $scope.selectedsubject = [];
                                        angular.forEach($scope.subjectlt, function (stu2) {
                                            if (stu2.checkedsub == true) {
                                                $scope.selectedsubject.push(stu2);
                                            }
                                        });
                                        $scope.selectedssubject = [];
                                        angular.forEach($scope.subsubjlt, function (stu3) {
                                            if (stu3.checked == true) {
                                                $scope.selectedssubject.push(stu3);
                                            }
                                        });

                                        $scope.selectedclass.push({
                                            year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                            emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: $scope.stusec,
                                            sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                        });

                                        $scope.details = false;
                                    }
                                });
                                $scope.clearpush_data();
                            }
                        }
                    }
                    if ($scope.all == 'st' || $scope.all == 'others') {
                        angular.forEach($scope.yearlt, function (stu) {
                            if ($scope.asmaY_Id == stu.asmaY_Id) {
                                $scope.yearname = stu.asmaY_Year;
                                $scope.yerId = stu.asmaY_Id;
                            }
                        });
                        angular.forEach($scope.emplt, function (stu) {
                            if ($scope.hrmE_Id == stu.hrmE_Id) {
                                $scope.empname = stu.hrmE_EmployeeFirstName;
                            }
                        });
                        angular.forEach($scope.usrlt, function (stu) {
                            if ($scope.ivrmulF_Id == stu.ivrmulF_Id) {
                                $scope.ivrmulF_Id = stu.ivrmulF_Id;
                            }
                        });

                        if ($scope.selectedclass.length > 0) {
                            var cnn = 0;
                            angular.forEach($scope.selectedclass, function (si) {
                                if (si.yearid == $scope.yerId && si.user_id == $scope.ivrmulF_Id) {
                                    cnn += 1;
                                    angular.forEach($scope.clslist, function (stu) {
                                        if (stu.checked == true) {
                                            $scope.selectedsection = [];
                                            angular.forEach($scope.seclist, function (stu1) {
                                                $scope.selectedsection = [];
                                                if (stu1.checked == true) {
                                                    $scope.selectedsection.push(stu1);
                                                    $scope.selectedsubject = [];

                                                    angular.forEach($scope.subjectlt, function (stu2) {
                                                        if (stu2.checkedsub == true) {
                                                            $scope.selectedsubject.push(stu2);
                                                        }
                                                    });

                                                    $scope.selectedssubject = [];
                                                    angular.forEach($scope.subsubjlt, function (stu3) {
                                                        if (stu3.checked == true) {
                                                            $scope.selectedssubject.push(stu3);
                                                        }
                                                    });


                                                    if (si.yearid == $scope.yerId && si.user_id == $scope.ivrmulF_Id && si.clas.asmcL_Id == stu.asmcL_Id
                                                        && si.secs.asmS_Id == stu1.asmS_Id) {
                                                        $scope.selectedclass.push({
                                                            year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                                            emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: stu1,
                                                            sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                                        });

                                                    } else {
                                                        $scope.selectedclass.push({
                                                            year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                                            emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: stu1,
                                                            sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                                        });
                                                    }
                                                    $scope.details = false;
                                                }
                                            });
                                        }
                                    });
                                    $scope.clearpush_data();
                                }
                            });
                            var temp_list = [];
                            angular.forEach($scope.selectedclass, function (ore) {
                                if (ore.clas != {} && ore.secs != [] && ore.sub != [] && ore.yearid != '' && ore.ssub != [] && ore.emp_name != '' && ore.user_id != '' && ore.elpflg != '') {
                                    temp_list.push(ore);
                                }
                            });
                            $scope.selectedclass = temp_list;
                            if (cnn == 0) {
                                angular.forEach($scope.yearlt, function (stu) {
                                    if ($scope.asmaY_Id == stu.asmaY_Id) {
                                        $scope.yearname = stu.asmaY_Year;
                                        $scope.yerId = stu.asmaY_Id;
                                    }
                                });
                                angular.forEach($scope.emplt, function (stu) {
                                    if ($scope.hrmE_Id == stu.hrmE_Id) {
                                        $scope.empname = stu.hrmE_EmployeeFirstName;
                                    }
                                });
                                angular.forEach($scope.usrlt, function (stu) {
                                    if ($scope.ivrmulF_Id == stu.ivrmulF_Id) {
                                        $scope.ivrmulF_Id = stu.ivrmulF_Id;
                                    }
                                });

                                angular.forEach($scope.clslist, function (stu) {
                                    if (stu.checked == true) {
                                        $scope.selectedsection = [];
                                        angular.forEach($scope.seclist, function (stu1) {
                                            if (stu1.checked == true) {
                                                $scope.selectedsection.push(stu1);
                                                $scope.selectedsubject = [];
                                                angular.forEach($scope.subjectlt, function (stu2) {
                                                    if (stu2.checkedsub == true) {
                                                        $scope.selectedsubject.push(stu2);
                                                    }
                                                });
                                                $scope.selectedssubject = [];
                                                angular.forEach($scope.subsubjlt, function (stu3) {
                                                    if (stu3.checked == true) {
                                                        $scope.selectedssubject.push(stu3);
                                                    }
                                                });

                                                $scope.selectedclass.push({
                                                    year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                                    emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: stu1,
                                                    sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                                });

                                                $scope.details = false;
                                            }
                                        });
                                    }
                                });
                            }
                        }
                        else {
                            angular.forEach($scope.clslist, function (stu) {
                                if (stu.checked == true) {
                                    $scope.selectedsection = [];
                                    angular.forEach($scope.seclist, function (stu1) {
                                        $scope.selectedsection = [];
                                        if (stu1.checked == true) {
                                            $scope.selectedsection.push(stu1);

                                            $scope.selectedsubject = [];
                                            angular.forEach($scope.subjectlt, function (stu2) {
                                                if (stu2.checkedsub == true) {
                                                    $scope.selectedsubject.push(stu2);
                                                }
                                            });
                                            $scope.selectedssubject = [];
                                            angular.forEach($scope.subsubjlt, function (stu3) {
                                                if (stu3.checked == true) {
                                                    $scope.selectedssubject.push(stu3);
                                                }
                                            });

                                            $scope.selectedclass.push({
                                                year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                                                emp_name: $scope.empname, elpflg: $scope.all, clas: stu, secs: stu1,
                                                sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                                            });

                                            $scope.details = false;
                                        }
                                    });
                                }
                            });
                            $scope.clearpush_data();
                        }
                    }
                    console.log($scope.selectedclass);
                }
                else {
                    swal("Not Selected any subject");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: ' Academic Year' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name' },
                { name: 'asmcL_ClassName', displayName: 'Class Name' },
                { name: 'asmC_SectionName', displayName: 'Section Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.elP_ActiveFlg === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;'
                        +
                        '<a ng-if="row.entity.elP_ActiveFlg === true" href="javascript:void(0)" ng-click="grid.appScope.EditExamLoginPrivilagesdata(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;'
                        +
                        '</div>'
                }
            ]
        };

        $scope.viewrecordspopup = function (employee) {
            $scope.editEmployee = employee.elP_Id;
            var pageid = $scope.editEmployee;
            apiService.create("ExamLoginPrivilages/getalldetailsviewrecords", employee).then(function (promise) {
                $scope.viewrecordspopupdisplay = promise.gtdetailsview;
            });
        };

        //fix the order drag
        //ConfigA is an Items
        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();
        $scope.getOrder = function (orderarray) {
            //dd
        };

        //to deactive the data

        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";

            if (deactiveRecord.elpsS_Id != 0) {
                if (deactiveRecord.elPss_ActiveFlg == true) {
                    mgs = "Deactive";
                    confirmmgs = "Deactivated";
                }
                else {
                    mgs = "Active";
                    confirmmgs = "Activated";
                }
            }
            if (deactiveRecord.elpsS_Id == 0) {
                if (deactiveRecord.elPs_ActiveFlg == true) {
                    mgs = "Deactive";
                    confirmmgs = "Deactivated";

                }
                else {
                    mgs = "Active";
                    confirmmgs = "Activated";
                }
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
                        apiService.create("ExamLoginPrivilages/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + ' Successfully');
                                }
                                else {
                                    swal('Record Not  Activated/Deactivated');
                                }

                                angular.forEach($scope.gridOptions.data, function (i) {
                                    if (i.elP_Id == deactiveRecord.elP_Id && i.asmcL_Id == deactiveRecord.asmcL_Id && i.asmS_Id == deactiveRecord.asmS_Id) {
                                        $scope.viewrecordspopup(i);
                                    }
                                });
                            });
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.clslist.some(function (options) {
                return options.checked;
            });
        };

        $scope.isOptionsRequired2 = function () {
            return !$scope.seclist.some(function (options) {
                return options.checked;
            });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.subjectlt.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.isOptionsRequired4 = function () {
            return !$scope.subsubjlt.some(function (options) {
                return options.checked;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ismS_SubjectName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.subsubjectsrch = "";
        $scope.searchsubsub = function (obj) {
            return angular.lowercase(obj.emsS_SubSubjectName).indexOf(angular.lowercase($scope.subsubjectsrch)) >= 0;
        };

        // to Edit Data
        $scope.EditExamLoginPrivilagesdata = function (EditRecord) {
            $scope.act = 'edit';
            $scope.selectedclass = [];
            $scope.dis = true;
            $scope.clldis = true;
            var MEditId = EditRecord.elP_Id;
            apiService.create("ExamLoginPrivilages/editdetails/", EditRecord).then(function (promise) {
                $scope.stusec = {};
                $scope.empname = promise.hrmE_EmployeeFirstName;
                $scope.ivrmulF_Id = promise.ivrmulF_Id;
                if (promise.editlist.length > 0) {
                    $scope.list = promise.editlist;
                    $scope.all = promise.editlist[0].elP_Flg;
                    $scope.selectedsection = [];
                    $scope.selectedsubject = [];
                    $scope.selectedssubject = [];
                    $scope.classname = [];
                    $scope.empname = promise.hrmE_EmployeeFirstName;
                    $scope.ivrmulF_Id = promise.ivrmulF_Id;
                    angular.forEach($scope.list, function (stu1) {
                        angular.forEach($scope.yearlt, function (stu) {
                            if (stu1.asmaY_Id == stu.asmaY_Id) {
                                $scope.yearname = stu.asmaY_Year;
                                $scope.yerId = stu.asmaY_Id;
                            }
                        });
                        angular.forEach($scope.clslist, function (stu) {

                            if (stu1.asmcL_Id == stu.asmcL_Id) {
                                $scope.asmcL_Id = stu.asmcL_Id;
                                $scope.asmcL_ClassName = stu.asmcL_ClassName;
                                $scope.classname = stu;
                            }
                        });

                        angular.forEach($scope.seclist, function (stu2) {
                            if (stu1.asmS_Id == stu2.asmS_Id) {
                                if ($scope.selectedsection.length == 0) {
                                    $scope.selectedsection.push({ asmC_SectionName: stu2.asmC_SectionName, asmS_Id: stu2.asmS_Id });
                                    $scope.stusec = stu2;
                                }
                                else if ($scope.selectedsection.length > 0) {
                                    var al_ct = 0;
                                    angular.forEach($scope.selectedsection, function (uf) {
                                        if (uf.asmC_SectionName == stu2.asmC_SectionName) {
                                            al_ct += 1;
                                        }
                                    });
                                    if (al_ct == 0) {
                                        $scope.selectedsection.push({ asmC_SectionName: stu2.asmC_SectionName, asmS_Id: stu2.asmS_Id });
                                        $scope.stusec = stu2;
                                    }
                                }

                            }
                        });
                        if ($scope.selectedsubject.length == 0) {
                            angular.forEach($scope.subjectlt, function (sb) {
                                if (stu1.ismS_Id == sb.ismS_Id) {
                                    $scope.selectedsubject.push(sb);
                                }
                            });
                        }
                        else if ($scope.selectedsubject.length > 0) {
                            var al_sub_cnt = 0;
                            angular.forEach($scope.selectedsubject, function (sb1) {

                                if (stu1.ismS_Id == sb1.ismS_Id) {
                                    al_sub_cnt += 1;
                                }
                            });
                            if (al_sub_cnt == 0) {
                                angular.forEach($scope.subjectlt, function (sb) {
                                    if (stu1.ismS_Id == sb.ismS_Id) {
                                        $scope.selectedsubject.push(sb);
                                    }
                                });
                            }
                        }
                        if ($scope.selectedssubject.length == 0) {
                            angular.forEach($scope.subsubjlt, function (sb) {
                                if (stu1.emsS_Id == sb.emsS_Id) {
                                    $scope.selectedssubject.push(sb);
                                }
                            });
                        }
                        else if ($scope.selectedssubject.length > 0) {
                            var al_sub_cnt1 = 0;
                            angular.forEach($scope.selectedssubject, function (sb1) {

                                if (stu1.emsS_Id == sb1.emsS_Id) {
                                    al_sub_cnt1 += 1;
                                }
                            });
                            if (al_sub_cnt1 == 0) {
                                angular.forEach($scope.subsubjlt, function (sb) {
                                    if (stu1.emsS_Id == sb.emsS_Id) {
                                        $scope.selectedssubject.push(sb);
                                    }
                                });
                            }
                        }
                        $scope.details = false;
                    });

                    $scope.selectedclass.push({
                        year: $scope.yearname, yearid: $scope.yerId, user_id: $scope.ivrmulF_Id,
                        emp_name: $scope.empname, elpflg: $scope.all, clas: $scope.classname, secs: $scope.stusec,
                        sub: $scope.selectedsubject, ssub: $scope.selectedssubject
                    });

                    $scope.clearpush_data();
                } else {
                    swal('NO RECORD FOUND');
                    $scope.clldis = false;
                    $scope.dis = false;

                }
            });

            $scope.clldis = true;
            console.log($scope.selectedclass);
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // TO Save The Data
        $scope.saveddata = function () {
            var data = {
                selectedclass: $scope.selectedclass,
                "action": $scope.act
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamLoginPrivilages/savedetails", data).then(function (promise) {
                if (promise.returnval == true || promise.returnduplicatestatus == "Duplicate") {
                    swal('Record Saved/Updated Successfully', 'success');
                    $scope.BindData();
                    $scope.details = true;
                    $scope.selectedclass = "";
                    $state.reload();
                }
                else if (promise.returnval == false) {
                    swal('Record Not Saved/Updated Successfully', 'Failed');
                    $scope.BindData();
                }
            });
        };

        $scope.cancel = function () {
            $scope.EME_ID = "";
            $scope.code = "";
            $scope.name = "";
            $scope.order = "";
            $scope.searchchkbx = "";
            $scope.subsubjectsrch = "";
            $state.reload();
        };

        $scope.clearpush_data = function () {
            angular.forEach($scope.clslist, function (stu) {
                stu.checked = false;
            });
            angular.forEach($scope.seclist, function (stu1) {
                stu1.checked = false;
            });

            angular.forEach($scope.subjectlt, function (stu2) {
                stu2.checkedsub = false;
            });

            angular.forEach($scope.subsubjlt, function (stu3) {
                stu3.checked = false;
            });

            if ($scope.all == 'ct') {
                $scope.classteacher();
            }
            else if ($scope.all == 'st') {
                $scope.subjectteacher();
            }
            else if ($scope.all == 'others') {
                $scope.others();
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.obj123.ivrmulF_Id = "";
            $scope.obj1234.hrmE_Id = "";
        };

        $scope.delete = function (row, index) {
            $scope.act = 'add';
            $scope.dis = false;
            $scope.asmaY_Id = "";
            $scope.ivrmulF_Id = "";
            $scope.hrmE_Id = "";
            angular.forEach($scope.clslist, function (stu) {
                stu.checked = false;
            });
            angular.forEach($scope.seclist, function (stu1) {
                stu1.checked = false;
            });

            angular.forEach($scope.subjectlt, function (stu2) {
                stu2.checkedsub = false;
            });

            angular.forEach($scope.subsubjlt, function (stu3) {
                stu3.checked = false;
            });
            for (var x = 0; x < $scope.selectedclass.length; x++) {
                if (x == index) {
                    $scope.selectedclass.splice(x, 1);
                }
            }
        };

        $scope.edit = function (row, index) {
            $scope.clearpush_data();
            $scope.dis = true;
            $scope.clldis = true;
            $scope.all = row.elpflg;
            $scope.asmaY_Id = row.yearid;
            $scope.usrlt = $scope.tempusrlt;
            $scope.ivrmulF_Id = row.user_id;
            angular.forEach($scope.usrlt, function (opt) {
                if (opt.ivrmulF_Id == $scope.ivrmulF_Id) {
                    $scope.usr_id = opt.emp_Code;
                    $scope.obj123.ivrmulF_Id = opt;
                }
            });

            $scope.emplt = $scope.tempemplt;
            angular.forEach($scope.emplt, function (opt) {
                if (opt.hrmE_Id == $scope.usr_id) {
                    $scope.hrmE_Id = $scope.usr_id;
                    $scope.obj1234.hrmE_Id = opt;
                    opt.Selected = true;
                }
            });

            angular.forEach($scope.clslist, function (opt) {
                if (opt.asmcL_Id == row.clas.asmcL_Id) {
                    opt.checked = true;
                }
            });
            angular.forEach($scope.seclist, function (opt) {
                if (opt.asmS_Id == row.secs.asmS_Id) {
                    opt.checked = true;
                }
            });

            angular.forEach($scope.subjectlt, function (opt) {
                angular.forEach(row.sub, function (sub) {
                    if (opt.ismS_Id == sub.ismS_Id) {
                        opt.checkedsub = true;
                    }
                });
            });

            angular.forEach($scope.subsubjlt, function (opt) {
                angular.forEach(row.ssub, function (ssub) {
                    if (opt.emsS_Id == ssub.emsS_Id) {
                        opt.checked = true;

                    }
                });
            });
        };

        $scope.OnAcdyear = function () {
            $scope.hrmE_Id = "";
            $scope.ivrmulF_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ElP_Flg": $scope.all
            };
            apiService.create("ExamLoginPrivilages/OnAcdyear", data).then(function (promise) {
                $scope.selectemplt = [];
                $scope.selectusrlt = [];
                $scope.temp_clsteas = promise.clastechlt;
                $scope.emplt = $scope.emplist;
                $scope.usrlt = $scope.userlist;
            });
        };
    }
})();