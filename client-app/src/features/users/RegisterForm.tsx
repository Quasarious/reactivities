import { ErrorMessage, Form, Formik } from "formik";
import React from "react";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Header } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup';
import ValidationErrors from "../errors/ValidationErrors";

export default observer( function RegisterForm() {
    const {userStore} = useStore();
    return (
        <Formik
            initialValues={{displayName: '', username: '', email: '', password: '', error: null}}
            onSubmit={(values, {setErrors}) => userStore.register(values).catch(error => 
                setErrors({error: error}))}
            validationSchema={Yup.object({
                displayName: Yup.string()
                .required("Имя должно быть заполнено")
                .min(3, 'Поле должно быть длиннее 2 символов')
                .max(50, 'Имя не может быть длиннее 50 символов!'),
                username: Yup.string()
                .required("Имя пользователя должно быть заполнено")
                .min(3, 'Поле должно быть длиннее 2 символов')
                .max(50, 'Имя пользователя не может быть длиннее 50 символов!'),
                email: Yup.string()
                .required("Почта должна быть заполнена")
                .email(),
                password: Yup.string()
                .required("Пароль не может быть пустым")
                .min(4, 'Введите пароль от 4 до 12 символов!')
                .max(12, 'Введите пароль от 4 до 12 символов!'),
            })}
        > 

            {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
                <Form className='ui form error' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Регистрация' color='teal' textAlign='center' />
                    <MyTextInput name='displayName' placeholder='Имя' />
                    <MyTextInput name='username' placeholder='никнейм' />
                    <MyTextInput name='email' placeholder='Электронная почта' />
                    <MyTextInput name='password' placeholder='Пароль' type='password'/>
                    <ErrorMessage 
                        name='error' render={() => 
                            <ValidationErrors errors={errors.error}/>}
                    />
                    <Button disabled={!isValid || !dirty || isSubmitting} 
                        loading={isSubmitting} positive content='Зарегистрироваться' type='submit' fluid/>
                </Form>
            )}
        </Formik>
    )
})