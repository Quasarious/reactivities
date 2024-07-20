import React, { useEffect, useState } from "react";
import { Button, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useHistory, useParams } from "react-router-dom";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import {v4 as uuid} from "uuid";
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";
import { categoryOptions } from "../../../app/common/options/categoryOptions";
import MySelectInput from "../../../app/common/form/MySelectInput";
import MyDateInput from "../../../app/common/form/MyDateInput";
import { Activity } from "../../../app/models/activity";

export default observer (function ActivityForm() {
    const history = useHistory();
    const {activityStore} = useStore();
    const {createActivity, updateActivity, loading, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    const[activity, setActivity] = useState<Activity>({
        id: '',
        title: '',
        category: '',
        description: '',
        date: null,
        city: '',
        venue: ''
    });

    const activityValidationSchema = Yup.object({
        title: Yup.string()
            .required('Заголовок не может быть пустым')
            .min(3, 'Поле должно быть длиннее 2 символов')
            .max(50, 'Заголовок не может быть длиннее 50 символов!'),
        category: Yup.string()
            .required('Категория не может быть пустой')
            .min(3, 'Категория должна быть длиннее 2 символов')
            .max(30, 'Категория не может быть длиннее 30 символов!'),
        description: Yup.string()
            .required('Описание не может быть пустым')
            .min(10, 'Описание должно быть длиннее 10 символов')
            .max(500, 'Описание не может быть длиннее 500 символов!'),
        date: Yup.date()
            .required('Дата не может быть пустой'),
        city: Yup.string()
            .required('Город не может быть пустым')
            .min(2, 'Город должен быть длиннее 1 символа')
            .max(50, 'Город не может быть длиннее 50 символов!'),
        venue: Yup.string()
            .required('Место проведения не может быть пустым')
            .min(2, 'Место проведения должно быть длиннее 1 символа')
            .max(100, 'Место проведения не может быть длиннее 100 символов!')
    });

    useEffect(() => {
        if (id) loadActivity(id).then(activity => setActivity(activity!))
    }, [id, loadActivity]);

    function handleFormSubmit(activity: Activity) {
        if (activity.id.length === 0) {
            let newActivity = {
                ...activity,
                id: uuid()
            };
            createActivity(newActivity).then(() => history.push(`/activities/${newActivity.id}`))
        } else {
            updateActivity(activity).then(() => history.push(`/activities/${activity.id}`))
        }
    }

    if (loadingInitial) return <LoadingComponent content='Loading activity...'/>

    return (
        <Segment clearing>
            <Header content='Информация о событии' sub color='teal'/>
            <Formik 
                    validationSchema={activityValidationSchema}
                    enableReinitialize 
                    initialValues={activity} 
                    onSubmit={values => handleFormSubmit(values)}>
                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='title' placeholder='Заголовок'/>
                        <MyTextArea rows={3} placeholder='Описание' name='description'/>
                        <MySelectInput options={categoryOptions} placeholder='Категория' name='category'/>
                        <MyDateInput 
                            placeholderText="Дата"
                            name='date' 
                            showTimeSelect
                            timeCaption='Время'
                            dateFormat='MMMM d, yyyy h:mm aa'
                        />
                        <Header content='Данные о месте проведения' sub color='teal'/>
                        <MyTextInput placeholder='Город' name='city'/>
                        <MyTextInput placeholder='Место' name='venue'/>
                        <Button 
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={loading} floated='right' 
                            positive type='submit' content='Подтвердить'/>
                        <Button as={Link} to='/activities' floated='right' type='button'content='Отмена'/>
                    </Form>
                )}
            </Formik>
        </Segment>
    )
}); 