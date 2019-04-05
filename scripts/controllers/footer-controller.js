angular.module('synapseWebApp').controller('AcaUpdateViewCtrl', function ($scope, $timeout, $window, $filter, $route, $routeParams, $log, $rootScope,
																																					sessionManagementService, uploadStorageService, $location, $modal, academicUpdatesGlobalService,
																																					campusSettingsService, restAPIPaginator, academicUpdatesService, ngTableParams,
																																					academicUpdateRequestListService, urlHistoryService, errorHandlingService) {

	function getUserRole() {
		var isCoordinator = sessionManagementService.isCurrentRoleCoordinator();
		if (sessionManagementService.isCurrentRoleCoordinator()) {
			var pathElements = $location.path().split("/");
			if (pathElements.length > 0 && (pathElements[1] === "academic-updates")) {
				isCoordinator = false;
			}
		}
		return isCoordinator;
	}

	var urlParams = urlHistoryService.newInstance({
		"sortBy": "",
		"pageSize": 25,
		"pageNo": 1
	}, "academicUpdate");

	urlParams.pageSize(25);//ESPRJ-12656 Page size should always be 25 if faculty leaves that view and come back

	$rootScope.$on('$routeChangeStart', function (event, next, prev) {
		var prevPath = prev.$$route.originalPath;
		if (prevPath.indexOf("studentprofile") === -1 && prevPath.indexOf("academic-updates") === -1) {
			urlHistoryService.clearInstance("academicUpdate");
		}
	});

	$scope.non_participant_count = 0;
	$scope.academicId = $routeParams.academicId;
	$scope.isAdhoc = false;
	$scope.isCoordinator = getUserRole();
	$scope.academicUpdate = {};
	$scope.requestParameter = '';
	$scope.MoreAttrbutes = {};
	$scope.totalAttr = 0;
	$scope.attrValue = 0;
	$scope.isShowMore = false;
	$scope.more = false;
	$scope.checkAll = [];
	$scope.checkAllStudent = false;
	$scope.totalStudentCount = 0;
	$scope.totalReadyCount = 0;
	$scope.refer_for_academic_assistance = false;
	$scope.colSpan = 4;
	var saveOrSend = "";

	$scope.campusMetadata = {};

	var academicUpdatesUpdate = false;
	$scope.sentEmail = function () {
		academicUpdatesGlobalService.academicSendReminder($scope.academicId).then(function (responce) {
			if (responce.errors === undefined || responce.errors.length === 0) {
				var message = "Reminder sent successfully";
				showMessage(message, false);
			} else {
				var message = responce.errors[0];
				showMessage(message, true);
			}
		}, function (errorText) {
			showMessage(errorText, true);
		});
	};

	function showMessage(message, error, isUpdate, complete) {

		MessageDialog.showMessage($modal, {
			"message": message,
			"error": error,
			"autoClose": !error,
			"onSubmit": function () {
				if ($scope.saveOrSend === "save") {
					$location.path('/academic-updates');
				} else if (!$scope.isAdhoc && complete !== "sentReferal") {
					$route.reload();
				}
				if (angular.isDefined(complete) && complete === "sentReferal") {
					sentReferal(isUpdate);
					if ($scope.isAdhoc) {
						angular.forEach($scope.tableParams.data[0].student_details, function (studentList) {
							studentList.student_refer = null;
							studentList.notSubmited = true;
						});
					}
				}
			}
		});
	}

	$scope.showMoreAttr = function () {
		$scope.more = true;
	};

	$scope.onModified = function (attr1, attr2) {
		if (attr2 !== undefined) {
			attr2.notSubmited = false;
			attr2.is_bypassed = false;
		}
	};
	$scope.close = function () {
		$scope.more = false;
	};

	$scope.setStudent_risk = function (student, level) {
		student.is_bypassed = false;
		student.notSubmited = false;
		if (student.student_risk == level) {
			student.student_risk = "";
		} else {
			student.student_risk = level;
		}
		$scope.onStudentDataChange(student);
	};
	$scope.isStudent_risk = function (student, level) {
		return (student.student_risk == level);
	};

	$scope.goToAcademicUpdatesList = function () {
		if ($scope.isCoordinator) {
			$location.path('/academic-updates-setup');
		} else {
			$location.path('/academic-updates');
		}
	};
	$scope.goToStudentProfilePage = function (student) {
		$location.path('/studentprofile/' + student.student_id);
	};

	$scope.goToStudentProfileCoursesPage = function (student) {
		academicUpdatesGlobalService.setIsOpenCoursesPage(true);
		$location.path('/studentprofile/' + student.student_id);
	};

	function fetchCourse() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.courses !== undefined) && ($scope.academicUpdate.request_attributes.courses.is_all == true || $scope.academicUpdate.request_attributes.courses.selected_course_ids != '')) {
			var courses = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.courses.selected_course_ids) &&
				$scope.academicUpdate.request_attributes.courses.selected_course_ids != '') {
				courses = $scope.academicUpdate.request_attributes.courses.selected_course_ids.split(',');
			}
			$scope.MoreAttrbutes.courses = {name: "Courses", more: []};
			academicUpdatesGlobalService.fetchAllActiveCourseList().then(function (responce) {
				angular.forEach(responce, function (course) {
					if ($scope.academicUpdate.request_attributes.courses.is_all == true) {
						$scope.MoreAttrbutes.courses.more.push(course.course_name);
					} else {
						angular.forEach(courses, function (group) {
							if (angular.equals(course.course_id, parseInt(group))) {
								$scope.MoreAttrbutes.courses.more.push(course.course_name);
							}
						});
					}
				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
	}

	function fetchGroups() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.groups !== undefined) && ($scope.academicUpdate.request_attributes.groups.is_all == true || $scope.academicUpdate.request_attributes.groups.selected_group_ids != '')) {
			var groupar = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.groups.selected_group_ids) &&
				$scope.academicUpdate.request_attributes.groups.selected_group_ids != '') {
				groupar = $scope.academicUpdate.request_attributes.groups.selected_group_ids.split(',');
			}
			$scope.MoreAttrbutes.groupNm = {name: "Groups", more: []};
			academicUpdatesGlobalService.fetchAllActiveGroupsList().then(function (responce) {
				angular.forEach(responce, function (groupNm) {
					if ($scope.academicUpdate.request_attributes.groups.is_all == true) {
						$scope.MoreAttrbutes.groupNm.more.push(groupNm.group_name);
					} else {
						angular.forEach(groupar, function (group) {
							if (angular.equals(groupNm.group_id, parseInt(group))) {
								$scope.MoreAttrbutes.groupNm.more.push(groupNm.group_name);
							}
						});
					}
				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
	}

	function fetchProfile() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.profile !== undefined) && ($scope.academicUpdate.request_attributes.profile.is_all == true || $scope.academicUpdate.request_attributes.profile.selected_ebi_ids != '')) {
			var profilear = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.profile.selected_ebi_ids) &&
				$scope.academicUpdate.request_attributes.profile.selected_ebi_ids != '') {
				profilear = $scope.academicUpdate.request_attributes.profile.selected_ebi_ids.split(',');
			}
			$scope.MoreAttrbutes.profileBk = {name: "Ebi profile items", more: []};
			academicUpdatesGlobalService.fetchAllEbiProfileList().then(function (responce) {
				angular.forEach(responce, function (profile) {

					angular.forEach(profilear, function (group) {
						if (angular.equals(parseInt(profile.id), parseInt(group))) {
							$scope.MoreAttrbutes.profileBk.more.push(profile.display_name);
						}
					});

				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.profile !== undefined) && ($scope.academicUpdate.request_attributes.profile.is_all == true || $scope.academicUpdate.request_attributes.profile.selected_isp_ids != '')) {
			var ispar = [];
			if ($scope.academicUpdate.request_attributes.profile.selected_isp_ids != '') {
				ispar = $scope.academicUpdate.request_attributes.profile.selected_isp_ids.split(',');
			}
			$scope.MoreAttrbutes.isps = {name: "ISPs", more: []};
			academicUpdatesGlobalService.fetchAllISPProfileList().then(function (responce) {
				angular.forEach(responce, function (isps) {

					angular.forEach(ispar, function (group) {
						if (angular.equals(isps.id, parseInt(group))) {
							$scope.MoreAttrbutes.isps.more.push(isps.display_name);
						}
					});

				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
	}

	function fetchStaff() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.staff !== undefined) && ($scope.academicUpdate.request_attributes.staff.is_all == true || $scope.academicUpdate.request_attributes.staff.selected_staff_ids != '')) {
			var staff = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.staff.selected_staff_ids) &&
				$scope.academicUpdate.request_attributes.staff.selected_staff_ids != '') {
				staff = $scope.academicUpdate.request_attributes.staff.selected_staff_ids.split(',');
			}
			$scope.MoreAttrbutes.staffs = {name: "Staffs", more: []};
			academicUpdatesGlobalService.fetchStaffList().then(function (responce) {
				$log.debug(responce);
				angular.forEach(responce, function (groupNm) {
					var staffName = groupNm.staff_lastname + ", " + groupNm.staff_firstname;
					if ($scope.academicUpdate.request_attributes.staff.is_all == true) {
						$scope.MoreAttrbutes.staffs.more.push(staffName);
					} else {
						angular.forEach(staff, function (group) {
							if (angular.equals(groupNm.staff_id, parseInt(group))) {
								$scope.MoreAttrbutes.staffs.more.push(staffName);
							}
						});
					}
				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
	}

	function fetchStudent() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.students !== undefined) && ($scope.academicUpdate.request_attributes.students.is_all == true || $scope.academicUpdate.request_attributes.students.selectedStudentIds != '')) {
			var students = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.students.selectedStudentIds) && $scope.academicUpdate.request_attributes.students.selectedStudentIds != '') {
				students = $scope.academicUpdate.request_attributes.students.selectedStudentIds.split(',');
			}
		}
		$scope.MoreAttrbutes.student = {name: "Student", more: []};
		academicUpdatesGlobalService.enableRefetchStudent();
		academicUpdatesGlobalService.fetchAllActiveStudentList($scope.academicUpdate.request_id).then(function (responce) {
			angular.forEach(responce, function (groupNm) {
				var staffName = groupNm.student_lastname + ", " + groupNm.student_firstname;
				if ($scope.academicUpdate.request_attributes.students.is_all) {
					$scope.MoreAttrbutes.student.more.push(staffName);
				} else {
					angular.forEach(students, function (student) {
						if (angular.equals(groupNm.student_id, parseInt(student))) {
							$scope.MoreAttrbutes.student.more.push(staffName);
						}
					});
				}
			});
			countTotalAttr();
		}, function (errorText) {
			errorHandlingService.showErrorMessage(errorText);
		});
	}


	function fetchStaticList() {
		if (($scope.academicUpdate !== undefined) && ($scope.academicUpdate.request_attributes !== undefined) && ($scope.academicUpdate.request_attributes.static_list !== undefined) && ($scope.academicUpdate.request_attributes.static_list.is_all == true || $scope.academicUpdate.request_attributes.static_list.selected_static_ids != '')) {
			var static_lists = [];
			if (angular.isDefined($scope.academicUpdate.request_attributes.static_list.selected_static_ids) &&
				$scope.academicUpdate.request_attributes.static_list.selected_static_ids != '') {
				static_lists = $scope.academicUpdate.request_attributes.static_list.selected_static_ids.split(',');
			}
			$scope.MoreAttrbutes.static_list = {name: "Static List", more: []};
			academicUpdatesGlobalService.fetchAllStaticList().then(function (response) {
				angular.forEach(response, function (staticList) {
					var staticListName = staticList.staticlist_name;
					if ($scope.academicUpdate.request_attributes.static_list.is_all) {
						$scope.MoreAttrbutes.static_list.more.push(staticListName);
					} else {
						angular.forEach(static_lists, function (static_list_id) {
							if (angular.equals(staticList.staticlist_id, parseInt(static_list_id))) {
								$scope.MoreAttrbutes.static_list.more.push(staticListName);
							}
						});
					}
				});
				countTotalAttr();
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		}
	}

	function countTotalAttr() {
		$scope.totalAttr = 0;
		$scope.attrValue = 0;
		$scope.isShowMore = false;
		$scope.requestParameter = '';
		var txt = '';
		var filterTypeText = '';
		var attrList = 0;
		var addMore = true;
		angular.forEach($scope.MoreAttrbutes, function (Attrbutes) {
			$scope.totalAttr += Attrbutes.more.length;
			if (addMore) {
				angular.forEach(Attrbutes.more, function (more, i) {
					if (addMore) {
						if (i === 0) {
							if (filterTypeText === '') {
								txt = Attrbutes.name + ": " + more;
							} else {
								txt = ", " + Attrbutes.name + ": " + more;
							}
						} else {
							txt = "/" + more;
						}
						if ((filterTypeText.length + txt.length) > 80) {
							if (filterTypeText.length === 0) {
								filterTypeText = $filter('limitTo')(txt, 80);
								attrList++;
							}
							addMore = false;
						} else {
							filterTypeText += txt;
							attrList++;
						}
					}
				});
			}
		});
		$scope.requestParameter = filterTypeText;
		$scope.attrValue = $scope.totalAttr - attrList;
		if ($scope.attrValue > 0) {
			$scope.isShowMore = true;
		}
	}

	function fetchfeatures() {
		$scope.getGlobalFeatureSettingsPromise = academicUpdatesGlobalService.getFeatureSettings().then(function (response) {
			$scope.campusMetadata = response;
			$scope.send_to_student = $scope.campusMetadata.send_to_student;
			if ($scope.send_to_student) {
				$scope.colSpan++;
			}
			if ($window.sessionStorage.isReferralOrgActive === "true" &&
				$window.sessionStorage.isReasonRouting === "true" &&
				$window.sessionStorage.reasonRoutedReferralsPublicShareCreate === "true" &&
				$scope.campusMetadata.refer_for_academic_assistance) {

				$scope.refer_for_academic_assistance = true;
				$scope.colSpan++;
			}
		}, function (errorText) {
			errorHandlingService.showErrorMessage(errorText);
		});
	}

	$scope.tableData = {};
	$scope.$watch('tableParams.$params.page', function (newParams, oldParams) {


		$scope.tableData.newParams = newParams;
		$scope.tableData.oldParams = oldParams;
	}, true);

	function renderTable(academicUpdatesUpdate) {

		if (angular.isDefined($scope.tableParams)) {
			$scope.tableParams.$params.page = 1;
			$scope.tableParams.reload();
		}
		else {
			$scope.tableParams = new ngTableParams({
					page: urlParams.pageNo(),            // show first page
					count: urlParams.pageSize()          // count per page
				},
				{
					counts: [25, 50, 100],
					getData: function ($defer, params) {
						if (academicUpdatesUpdate && $scope.readyToSendCount() > 0) {
							if (!$scope.changeDueToSubmission) {
								if ($scope.changeDueToReset) {
									$scope.changeDueToReset = false;
									return;
								}
								$scope.changeDueToReset = false;
								$scope.confirmWindowForDelete();
								return;
							} else {
								$scope.changeDueToSubmission = false;
							}
						}
						var paginatorParams = {
							pageNo: params.page(),
							offset: params.count(),
							user_type: academicUpdatesGlobalService.getUserRoleName()
						};
						urlParams.pageNo(params.page());
						urlParams.pageSize(params.count());
						if ($scope.isAdhoc) {
							$scope.totalPages = Math.ceil($scope.academicUpdate.request_details[0].student_details.length / 25);
							$scope.currentPageIndex = params.page() - 1;
							$scope.totalStudentCount = $scope.numberOfRecords = $scope.academicUpdate.request_details[0].student_details.length;
							if (!angular.isDefined($scope.academicUpdate.request_details[0].student_details.non_participant_count) &&
								$window.sessionStorage.total_student_count_selected_course != $scope.totalStudentCount) {
								$scope.non_participant_count = $window.sessionStorage.total_student_count_selected_course - $scope.totalStudentCount;
							}
							$scope.range = $scope.numberOfRecords < params.count() ? $scope.numberOfRecords : params.count();

							if (angular.isDefined($scope.totalPages) && $scope.totalPages > 0) {
								if (params.page() > $scope.totalPages) {
									params.page($scope.totalPages);
								}
							}

							params.total($scope.numberOfRecords);
							if ($scope.academicUpdate.request_details) {
								var tableData = angular.copy($scope.academicUpdate.request_details);
								var startIndex = paginatorParams.pageNo == 1 ? 0 : (((paginatorParams.pageNo - 1) * (paginatorParams.offset)));
								var endIndex = (paginatorParams.pageNo * (paginatorParams.offset));
								endIndex = endIndex > $scope.numberOfRecords ? $scope.numberOfRecords : endIndex;
								var studentDetails = [];
								for (var i = startIndex; i < endIndex; i++) {
									studentDetails.push($scope.academicUpdate.request_details[0].student_details[i]);
								}
								tableData[0].student_details = studentDetails;
							} else {
								var tableData = [];
							}
							$defer.resolve(tableData);
							return;
						}

						if ($scope.selectedFilter.notSubmited === "All") {
							paginatorParams.filter = "All";
						} else {
							if ($scope.selectedFilter.notSubmited) {
								paginatorParams.filter = "nodata";
							} else {
								paginatorParams.filter = "datasubmitted";
							}
						}
						$scope.refetchSearchDataPromise = $scope.paginator.fetch(paginatorParams).then(function (response) {
							if (response && response.data) {
								$scope.totalStudentCount = response.data.total_records;
								$scope.non_participant_count = response.data.non_participant_count;
								$scope.totalPages = response.data.total_pages;

								$scope.numberOfRecords = $scope.paginator.noOfRecords();
								if ($scope.tableParams.$params.count != parseInt(angular.element('#table_params').text())) {
									angular.element('#table_params_id').text(urlParams.pageSize());
								}
								$scope.range = $scope.numberOfRecords < params.count() ? $scope.numberOfRecords : params.count();

								if (angular.isDefined($scope.totalPages) && $scope.totalPages > 0) {
									if (params.page() > $scope.totalPages) {
										params.page(response.data.total_pages);
									}
								}
								$scope.currentPageIndex = params.page() - 1;
								angular.forEach(response.data.data.request_details, function (requestDetails, key) {
									$scope.checkAll[key + $scope.currentPageIndex] = false;
									angular.forEach(requestDetails.student_details, function (student_details) {
										if (!(student_details.student_absences.length <= 0 && student_details.student_comments.length <= 0 &&
											student_details.student_risk.length <= 0 && student_details.student_refer == 0 &&
											student_details.student_refer == false && student_details.student_grade.length <= 0 && student_details.student_send == false && !$scope.canSendToStudent(student_details))) {
											student_details.save_for_later = true;
										}
										else {
											student_details.save_for_later = false;
										}
									});
								});
								params.total($scope.paginator.noOfRecords());
								if (response.data.data && response.data.data.request_details) {
									var data = response.data.data.request_details;
								} else {
									var data = [];
								}
								$defer.resolve(data);
								if (academicUpdatesUpdate) {
									academicUpdatesGlobalService.setNotSubmited(response.data.data, 'update');
								} else {
									academicUpdatesGlobalService.setNotSubmited(response.data.data);
									$scope.academicUpdate = angular.copy(response.data.data);
									fetchCourse();
									fetchGroups();
									fetchProfile();
									fetchStaff();
									fetchStudent();
									fetchStaticList();
								}

								$scope.academicUpdateBackup = response.data.data;
								$scope.academicUpdate = angular.copy($scope.academicUpdateBackup);
							} else {

								$defer.resolve(null);
								showMessage("Request has timed out, please try again later", true);
							}

						}, function (response) {
							errorHandlingService.showErrorMessage(errorHandlingService.prepareErrorMessage(response));
						});
					}
				});
		}
	}

	$scope.confirmWindowForDelete = function () {
		var message = "Changes have been made on the page, do you want to submit and proceed or cancel to stay on the page?";
		var dialogSettings = {
			title: "Confirm Page Submission",
			message: message,
			confirmButtonText: "Submit",
			onConfirm: function () {
				$scope.sendUpdate(academicUpdatesUpdate);
			},
			onCancel: function () {
				if ($scope.tableParams.$params.count == urlParams.pageSize()) {
					$scope.tableParams.$params.page = $scope.tableData.oldParams;
				}
				$scope.changeDueToReset = true;
				$scope.changeDueToSubmission = false;

				$scope.tableParams.$params.count = urlParams.pageSize();
				angular.element('#table_params_id').text(urlParams.pageSize());
			}
		};
		MessageDialog.showConfirmationDialog($modal, dialogSettings);
	};

	$scope.print = function (data) {
		console.log(data);
	};

	function fetchData() {
		fetchfeatures();
		var location = $location.path().split('/');
		location.splice(location.length - 1, 1);
		var loc = location.join('/');
		if (loc === "/academic-updates/update") {
			academicUpdatesUpdate = true;
		}
		if (!academicUpdatesUpdate) {

			$scope.paginator = restAPIPaginator.paginateAPI(
				academicUpdatesService,
				{
					user_type: academicUpdatesGlobalService.getUserRoleName(),
					orgId: $window.sessionStorage.organization_id,
					request: $scope.academicId,
					filter: "all",
					"output-format": "json"
				},
				{});
			renderTable(academicUpdatesUpdate);

		} else {
			$scope.paginator = restAPIPaginator.paginateAPI(
				academicUpdatesService,
				{
					user_type: academicUpdatesGlobalService.getUserRoleName(),
					orgId: $window.sessionStorage.organization_id,
					request: $scope.academicId,
					filter: "all",
					"output-format": "json"
				},
				{});
			$scope.selectedFilter.notSubmited = true;
			renderTable(academicUpdatesUpdate);
		}
	}

	$scope.changeSelection = function () {
		renderTable();
	};
	
	function getAdhocData() {
		fetchfeatures();
		academicUpdatesUpdate = true;
		$scope.isAdhoc = true;
		$scope.academicUpdateBackup = academicUpdatesGlobalService.getAdhocRequestData();
		if (angular.equals($scope.academicUpdateBackup, {})) {
			$location.path("/my-course");
		} else {
			renderAcademicUpdateReqPage();
		}
	}

	function renderAcademicUpdateReqPage() {
		$scope.academicUpdate = angular.copy($scope.academicUpdateBackup);

		angular.forEach($scope.academicUpdate.request_details[0].student_details, function (value) {
			value.student_refer = false;
			value.student_send = false;
			value.notSubmited = true;
		});
		if ($scope.academicUpdate.request_details[0].student_details.length > 1) {
			$scope.headerText = "Academic updates for " + $scope.academicUpdate.request_details[0].subject_course;
		} else {
			$scope.headerText = "Academic update for " + $scope.academicUpdate.request_details[0].student_details[0].student_lastname + ' ' + $scope.academicUpdate.request_details[0].student_details[0].student_firstname;
		}

		renderTable(academicUpdatesUpdate);
	}

	$scope.grade_list = [
		{"key": 'P', "value": "P/Pass"},
		{"key": 'A', "value": "A"},
		{"key": 'B', "value": "B"},
		{"key": 'C', "value": "C"},
		{"key": 'D', "value": "D"},
		{"key": 'F', "value": "F/Fail"}
	];
	$scope.requestFilters = [
		{"notSubmited": "All", "value": $filter('translate')('ACADEMIC_UPDATE_ALL_STUDENT')},
		{"notSubmited": false, "value": $filter('translate')('COMPLETE')},
		{"notSubmited": true, "value": "Incomplete"}
	];
	$scope.selectedFilter = $scope.requestFilters[0];
	if ($scope.academicId !== "0") {
		fetchData();
	} else {
		getAdhocData();
	}
	$scope.checkAllSendTo = function (elt) {
		var status = "";
		if ($scope.checkAll[elt.indexCount + $scope.currentPageIndex] === undefined) {
			status = $scope.checkAll[elt.indexCount + $scope.currentPageIndex] = true;
		}
		status = $scope.checkAll[elt.indexCount + $scope.currentPageIndex];
		angular.forEach(elt.student_details, function (student) {
			student.student_send = status;
		});
	};
	$scope.isCheckAllSendTo = function (elt) {
		var status = true;
		angular.forEach(elt.student_details, function (student) {
			if (student.student_send === false) {
				status = false;
			}
		});
		$scope.checkAll[elt.indexCount + $scope.currentPageIndex] = status;
	};

	$scope.updateAllstudent = function () {
		if ($scope.checkAllStudent === true) {
			angular.forEach($scope.tableParams.data, function (request) {
				angular.forEach(request.student_details, function (student) {
					if (!student.is_bypassed && student.notSubmited) {
						student.student_risk = "Low";
						student.is_bypassed = true;
						student.notSubmited = false;
					}
				});
				$scope.isCheckAllSendTo(request);
			});
		} else {
			angular.forEach($scope.tableParams.data, function (request) {
				angular.forEach(request.student_details, function (student) {
					if (student.is_bypassed) {
						student.student_risk = "";
						student.is_bypassed = false;
						student.notSubmited = true;
					}
				});
				$scope.isCheckAllSendTo(request);
			});
		}
	};

	$scope.readyToSendCount = function () {
		var totalReadyCount = 0;
		if (!$scope.tableParams) {
			return totalReadyCount;
		}
		angular.forEach($scope.tableParams.data, function (request) {
			angular.forEach(request.student_details, function (student) {
				if (student.notSubmited === false) {
					totalReadyCount++;
				}
			});
		});
		$scope.totalReadyCount = totalReadyCount;
		return totalReadyCount;
	};

	$scope.onStudentDataChange = function (student) {
		student.is_bypassed = false;
		student.notSubmited = student.student_risk.length <= 0 && student.student_grade.length <= 0 && student.student_absences.length <= 0 && student.student_comments.length <= 0 && !student.student_refer;

		if (student.student_absences === '' && student.student_comments === '') {
			student.student_send = false;
		}
	};
	$scope.changeReferTo = function (student) {
		if (student.student_refer) {
			student.is_bypassed = false;
			student.notSubmited = false;
		} else {
			if (student.student_risk === '' && student.student_grade === '' && student.student_absences === '' && student.student_comments === '' && !student.student_refer) {
				student.notSubmited = true;
			}
		}
	};

	$scope.exportAcademicUpdates = function () {
		$log.debug($scope.selectedFilter);
		var filter = "all";
		if ($scope.selectedFilter.notSubmited === "All") {
			filter = "all";
		} else if ($scope.selectedFilter.notSubmited) {
			filter = "nodata";
		} else {
			filter = "datasubmitted";
		}
		academicUpdatesGlobalService.fetchExportURL($scope.academicId, filter).then(function (exportURL) {
			var exportData = exportURL.data.URL.data;
			var path = exportData.split('/');
			var url = uploadStorageService.getUrl('roaster_uploads', path[path.length - 1]);
			$window.open(url, "_parent");
		}, function (errorText) {
			errorHandlingService.showErrorMessage(errorText);
		});
	};

	function assignReferForAcademicAssistance() {
		angular.forEach($scope.tableParams.data, function (request) {
			angular.forEach(request.student_details, function (student) {
				student.student_academic_assist_refer = student.student_refer;
			});
		});
	}

	$scope.saveForLater = function () {
		assignReferForAcademicAssistance();
		var requestDetails = angular.copy($scope.tableParams.data);

		//manually copy values corrupted by angular.copy
		for (var i = 0; i < $scope.tableParams.data.length; i++) {
			for (var j = 0; j < $scope.tableParams.data[i].student_details.length; j++) {
				if ($scope.tableParams.data[i].student_details[j].student_absences != '' && $scope.tableParams.data[i].student_details[j].student_absences.length != 0) {
					requestDetails[i].student_details[j].student_absences = parseInt($scope.tableParams.data[i].student_details[j].student_absences);
				}
				if (!(requestDetails[i].student_details[j].student_absences.length <= 0 &&
					requestDetails[i].student_details[j].student_comments.length <= 0 &&
					requestDetails[i].student_details[j].student_risk.length <= 0 &&
					requestDetails[i].student_details[j].student_refer == 0 &&
					requestDetails[i].student_details[j].student_refer == false &&
					requestDetails[i].student_details[j].student_grade.length <= 0 &&
					requestDetails[i].student_details[j].student_send == false && !$scope.canSendToStudent(requestDetails[i].student_details[j])) && requestDetails[i].student_details[j].save_for_later != true) {
					requestDetails[i].student_details[j].save_for_later = true;
				}
			}
		}

		var academic = $scope.academicUpdate;
		var sentData = {
			"request_id": academic.request_id,
			"request_name": academic.request_name,
			"request_description": academic.request_description,
			"request_created": academic.request_created,
			"request_due": academic.request_due,
			"request_complete_status": academic.request_complete_status,
			"save_type": "save",
			"request_details": requestDetails
		};
		saveOrSend = "save";
		sendAcademicUpdates(sentData);
	}


	$scope.sendUpdate = function (isUpdate) {
		var data = {
			"courses_with_academic_updates": []
		};
		var students_with_academic_updates;
		assignReferForAcademicAssistance();
		var courses = [];
		if ($scope.isAdhoc) {
			courses[0] = $scope.tableParams.data[0];
			courses[0].course_id = $window.sessionStorage.auReqCourseId;
		} else {
			courses = $scope.tableParams.data;
		}
		angular.forEach(courses, function (course) {
			students_with_academic_updates = [];
			angular.forEach(course.student_details, function (studentDetail) {
				academic_update = [];
				if (studentDetail.student_grade === 'P') {
					studentDetail.student_grade = 'Pass';
				}
				if (studentDetail.student_risk !== "" || studentDetail.student_comments !== "" || studentDetail.student_grade !== "" || studentDetail.student_absences !== "" || studentDetail.student_refer === 1 || studentDetail.student_refer === true) {
					var adhocAcademicUpdatePostBody = {};
					adhocAcademicUpdatePostBody.faculty_id_submitted = $window.sessionStorage.id;
					adhocAcademicUpdatePostBody.date_submitted = $filter('date')(new Date(), 'yyyy-MM-ddTHH:mm:ssZ');
					adhocAcademicUpdatePostBody.send_to_student = studentDetail.student_send && $scope.canSendToStudent(studentDetail);
					if (studentDetail.student_risk !== "") {
						adhocAcademicUpdatePostBody.failure_risk_level = studentDetail.student_risk;
						studentDetail.student_risk = "";
					}
					if (studentDetail.student_grade !== "") {
						adhocAcademicUpdatePostBody.in_progress_grade = studentDetail.student_grade;
						studentDetail.student_grade = "";

					}
					if (studentDetail.student_absences !== "") {
						adhocAcademicUpdatePostBody.absences = studentDetail.student_absences;
						studentDetail.student_absences = "";
					}
					if (studentDetail.student_comments !== "") {
						adhocAcademicUpdatePostBody.comment = studentDetail.student_comments;
						studentDetail.student_comments = "";
					}
					adhocAcademicUpdatePostBody.refer_for_assistance = studentDetail.student_refer;

					studentDetail.student_send = null;

					academic_update.push(adhocAcademicUpdatePostBody);

					if (academic_update.length > 0) {
						students_with_academic_updates.push({
							"student_id": studentDetail.student_id,
							"academic_updates": academic_update
						});
					}
				}
			});

			if (students_with_academic_updates.length > 0) {
				data.courses_with_academic_updates.push({
					"course_id": course.course_id,
					"students_with_academic_updates": students_with_academic_updates
				});
			}
		});
		saveOrSend = "send";
		data.save_type = saveOrSend;
		sendAcademicUpdates(data, isUpdate);

	};


	function sendAcademicUpdates(sentData, isUpdate) {
		angular.forEach(sentData.request_details, function (request, rkey) {
			var changedData = [];
			angular.forEach(request.student_details, function (item, key) {
				if (item.save_for_later) {
					changedData.push(item);
				} else if (!$scope.isAdhoc
					&& sentData.save_type === "send"
					&& $scope.academicUpdate.request_details
					&& $scope.academicUpdate.request_details[rkey]
					&& $scope.academicUpdate.request_details[rkey].student_details
				) {
					angular.forEach($scope.academicUpdate.request_details[rkey].student_details, function (checkItem) {
						if (checkItem.student_id === item.student_id && !checkItem.notSubmited) {
							changedData.push(item);
						}
					});
				}
				if (item.student_risk.length <= 0) {
					item.student_risk = null;
				}
			});

			request.student_details = changedData;
		});

		if ($scope.isAdhoc) { //Adhoc Submit
			$scope.sendAcademicUpdatePromise = academicUpdatesGlobalService.submitAcademicUpdatesAdhoc(sentData).then(function (response) {
				$log.debug(response);
				var message;
				if (response.errors === undefined || response.errors.length === 0) {
					$scope.saveOrSend = saveOrSend;

					message = "Your academic update has been submitted";
					showMessage(message, false, isUpdate, "sentReferal");


					$scope.academicUpdateBackup = angular.copy($scope.academicUpdate);

				} else {
					$scope.totalReadyCount = 0;
					message = response.errors[0].user_message;
					showMessage(message, true);
					$route.reload();
				}
			}, function (errorText) {
				errorHandlingService.showErrorMessage(errorText);
			});
		} else {

			if (sentData.save_type === "send") { //Submit
				academicUpdatesGlobalService.submitAcademicUpdates(sentData).then(function (response) {
					var message;
					if (response.errors === undefined || response.errors.length === 0) {
						$scope.saveOrSend = saveOrSend;

						message = "Your academic update has been submitted";
						showMessage(message, false, isUpdate, "sentReferal");
						$scope.academicUpdateBackup = angular.copy($scope.academicUpdate);
					} else {
						$scope.totalReadyCount = 0;
						message = response.errors[0].user_message;
						showMessage(message, true);
					}
				});
			} else { //Save
				academicUpdatesGlobalService.saveAcademicUpdates(sentData).then(function (response) {
					$scope.saveOrSend = saveOrSend;
					var message;
					if (response.errors === undefined || response.errors.length === 0) {
						$scope.saveOrSend = saveOrSend;
						message = "Request has been saved successfully";
						showMessage(message, false, isUpdate);

						$scope.academicUpdateBackup = angular.copy($scope.academicUpdate);

					} else {
						$scope.totalReadyCount = 0;
						message = response.errors[0].user_message;
						showMessage(message, true);
					}
				}, function (errorText) {
					errorHandlingService.showErrorMessage(errorText);
				});
			}
		}

		$scope.checkAllStudent = false;
	}

	$scope.canSendToStudent = function (student) {
		var output;
		output = (
			student.student_grade !== '' ||
			student.student_absences !== '' ||
			student.student_comments !== ''
		);
		return output;
	};

	$scope.resetAllData = function () {
		$scope.academicUpdate = angular.copy($scope.academicUpdateBackup);
		$scope.checkAllStudent = false;
		if (!$scope.isAdhoc) {
			$location.path('/academic-updates');
		} else {
			$location.path('/my-course');
		}
	};

	$scope.printAcademicUpdates = function () {
		var printHtml = $('#printSection').html();
		var windowUrl = '';
		var uniqueName = new Date();
		var windowName = 'Print' + uniqueName.getTime();

		var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,fullscreen=1');
		printWindow.document.write(printHtml);
		printWindow.document.close();
		printWindow.focus();
		$timeout(function () {
			printWindow.print();
			printWindow.close();
		}, 100);

	};

	function sentReferal(isUpdate) {
		var studentList = [];
		var updatesSent = 0;
		angular.forEach($scope.tableParams.data, function (request) {
			angular.forEach(request.student_details, function (student) {
				if (student.student_refer) {
					studentList.push({
						'id': student.student_id,
						'name': student.student_lastname + ' ' + student.student_firstname
					});
				}
				if (student.notSubmited === false) {
					updatesSent++;
				}
			});
		});
		if (studentList.length === 0) {
			if (!$scope.isAdhoc) {
				if (angular.isDefined(isUpdate) && isUpdate) {
					$scope.tableParams.$params.page = $scope.tableData.newParams;
					$scope.changeDueToSubmission = true;
					$scope.tableParams.reload();
					return;
				}

				if ($scope.isCoordinator) {
					$location.path('/academic-updates-setup');
				} else {
					if ($scope.totalStudentCount > $scope.readyToSendCount()) {
						$route.reload();
					} else {
						$location.path('/academic-updates');
					}
				}
			} else {
				if (angular.isDefined(isUpdate) && isUpdate) {
					$scope.tableParams.$params.page = $scope.tableData.newParams;
					$scope.changeDueToSubmission = true;
					$scope.tableParams.reload();
					return;
				} else {
					$location.path('/my-course');
				}
			}
		} else {
			var modalInstance = $modal.open({
				templateUrl: 'partials/academic-updates/refer-for-academic-assistance.html',
				backdrop: 'static',
				windowClass: 'academic-assistance-referral-academic',
				controller: 'AcaUpdateSentReferalCtrl',
				resolve: {
					data: function () {
						return {studentList: studentList, updatesSent: updatesSent};
					}
				}
			});
			modalInstance.result.then(function (result) {
				if (result && result.success) {
					if (angular.isDefined(isUpdate) && isUpdate) {
						$scope.tableParams.$params.page = $scope.tableData.newParams;
						$scope.changeDueToSubmission = true;
						$scope.tableParams.reload();
					} else {
						if (!$scope.isAdhoc) {
							$window.location.assign('/#/academic-updates');
							$route.reload();
						} else {
							$window.location.assign('/#/my-course');
							$route.reload();
						}
					}

				} else {
					$scope.tableParams.$params.page = $scope.tableData.newParams;
					$scope.changeDueToSubmission = true;
					$scope.tableParams.reload();
				}
			});
		}
	}
});
