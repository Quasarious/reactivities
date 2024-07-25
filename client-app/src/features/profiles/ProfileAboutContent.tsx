import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { Button, Grid, Header, TabPane } from "semantic-ui-react";
import ProfileEditForm from "./form/ProfileEditForm";
import { useStore } from "../../app/stores/store";

export default observer (function ProfileAboutContent() {    
    const {profileStore} = useStore();
    const {isCurrentUser, profile} = profileStore;
    const [editing, setEditing] = useState(false);

    function handleEditing() {
        setEditing(!editing);
    }

    return (
        <TabPane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='user' content={`О ${profile?.displayName}`} />
                    {isCurrentUser &&
                        <Button 
                            basic 
                            floated="right"
                            onClick={handleEditing}
                            content={editing ? 'Отмена' : 'Изменить'} 
                        />
                    }
                </Grid.Column>
                <Grid.Column width={16}>
                    {editing ? (<ProfileEditForm handleEditing={handleEditing}/>) 
                        : (<span style={{whiteSpace: 'pre-wrap'}}>{profile?.bio}</span>)
                    }
                </Grid.Column>
            </Grid>
        </TabPane>
    )
       
})