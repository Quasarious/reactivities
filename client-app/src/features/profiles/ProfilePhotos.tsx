import { observer } from "mobx-react-lite";
import React, { SyntheticEvent, useState } from "react";
import { Card, Header, TabPane, Image, Grid, Button } from "semantic-ui-react";
import { Photo, Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import PhotoUploadWidget from "../../app/common/imageUpload/PhotoUploadWidget";

interface Props {
    profile: Profile;
}

export default observer (function ProfilePhotos({profile}: Props) {
    const {profileStore: {isCurrentUser, uploadPhoto, uploading, 
                        loading, setMainPhoto, deletePhoto}} = useStore();
    const [addPhotoMode, setAddPhotoMode] = useState(false);
    const [target, setTarget] = useState('');

    function handleSetMainPhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        setMainPhoto(photo);
    }
    
    function handlePhotoUpload(file: Blob) {
        uploadPhoto(file).then(() => setAddPhotoMode(false));
    }

    function handleDeletePhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        deletePhoto(photo);
    }

    return (
        <TabPane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='image' content='Фото'/>
                    {isCurrentUser && (
                        <Button floated="right" basic
                                content={addPhotoMode? 'Отменить' : 'Добавить фото'}
                                onClick={() => setAddPhotoMode(!addPhotoMode)}
                        />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {addPhotoMode ? (
                        <PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={uploading}/>
                    ) : (
                        <Card.Group itemsPerRow={5}>
                        {profile.photos?.map(photo => (
                            <Card key={photo.id}>
                                <Image src={photo.url}/>
                                {isCurrentUser && (
                                    <Button.Group fluid widths={2}>
                                        <Button 
                                            variant='outlined'
                                            icon='check'
                                            basic
                                            color="green"
                                            name={'main' + photo.id}
                                            disabled={photo.isMain}
                                            loading={target ==='main' + photo.id && loading}
                                            onClick={e => handleSetMainPhoto(photo, e)}                                        />
                                        <Button 
                                            basic 
                                            color='red'
                                            name={photo.id}
                                            disabled={photo.isMain}
                                            icon='trash' 
                                            loading={target === photo.id && loading}
                                            onClick={e => handleDeletePhoto(photo, e)}
                                        />
                                    </Button.Group>
                                )}
                            </Card>
                        ))}
                    </Card.Group>
                    )}
                </Grid.Column>
            </Grid>
        </TabPane>
    )
})