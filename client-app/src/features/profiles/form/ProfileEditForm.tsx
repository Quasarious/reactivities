import { observer } from "mobx-react-lite";
import React from "react";
import MyTextInput from "../../../app/common/form/MyTextInput";
import { Button, Form } from "semantic-ui-react";
import { Formik} from "formik";
import { useParams } from "react-router-dom";
import { useStore } from "../../../app/stores/store";
import * as Yup from 'yup';
import MyTextArea from "../../../app/common/form/MyTextArea";
import { Profile } from "../../../app/models/profile";

interface Props {
    handleEditing: () => void;
}

export default observer (function ProfleEditForm({handleEditing}: Props) {
    const {profileStore} = useStore();
    const {updateProfile, profile} = profileStore;
    const {username} = useParams<{username: string}>();

    const profileValidationSchema = Yup.object({
        displayName: Yup.string()
            .required('Отображаемое имя не может быть пустым')
            .min(3, 'Поле должно быть длиннее 2 символов')
            .max(50, 'Имя не может быть длиннее 50 символов!'),
        bio: Yup.string()
            .max(5000, 'Категория не может быть длиннее 5000 символов!'),
    });
    
    function handleFormSubmit(profile: Profile) {
        profile.username = username;
        updateProfile(profile).then(handleEditing);
    }

    return (
            <Formik
                validationSchema={profileValidationSchema}
                enableReinitialize 
                initialValues={{displayName: profile?.displayName, bio:profile?.bio}}
                onSubmit={values => handleFormSubmit(values as Profile)}
            >
                    {({handleSubmit, isSubmitting, isValid, dirty}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='displayName' placeholder='Отображаемое имя'/>
                        <MyTextArea rows={10} placeholder='Описание' name='bio'/>
                        <Button 
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={isSubmitting} floated='right' 
                            positive type='submit' content='Подтвердить'/>
                    </Form>
                )}
            </Formik>
    )
})