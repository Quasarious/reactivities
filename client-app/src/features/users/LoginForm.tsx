import { ErrorMessage, Form, Formik } from "formik";
import React from "react";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Header, Label } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";

export default observer( function LoginForm() {
    const {userStore} = useStore();
    return (
        <Formik
            initialValues={{email: '', password: '', error: null}}
            onSubmit={(values, {setErrors}) => userStore.login(values).catch(error => setErrors({error: 'Инвалидный емэйл или пароль'}))}
        >

            {({handleSubmit, isSubmitting, errors}) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Авторизация' color='teal' textAlign='center' />
                    <MyTextInput name='email' placeholder='Email' />
                    <MyTextInput name='password' placeholder='Пароль' type='password'/>
                    <ErrorMessage 
                        name='error' render={() => <Label style={{marginBottom: 10}} basic color='red' content={errors.error}/>}
                    />
                    <Button loading={isSubmitting} positive content='Войти' type='submit' fluid/>
                </Form>
            )}
        </Formik>
    )
})